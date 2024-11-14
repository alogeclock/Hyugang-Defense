using System.Collections;
using System.Collections.Generic;
using System.Runtime;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public RuntimeAnimatorController[] animCon; 

    public float health;
    public int maxHealth;
    public int damage;

    public float speed;
    public float attackCooldown;
    float attackTimer;

    bool isLive;

    Animator anim;
    SpriteRenderer spriter;
    BoxCollider2D coll;
    Rigidbody2D rigid; // enemy's current position

    void Awake() 
    {
        rigid = GetComponent<Rigidbody2D>();
        spriter = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        coll = GetComponent<BoxCollider2D>();
        // anim.SetBool("Dead", false);
    }

    void OnEnable()
    {
        attackTimer = Mathf.Clamp(attackTimer - Time.deltaTime, 0, attackCooldown);
        isLive = true;
        health = maxHealth;
        coll.enabled = true;
        rigid.simulated = true;
    }

    void OnCollisionStay2D(Collision2D other)
    {
        if (!isLive) return;
        Unit unit = other.collider.GetComponent<Unit>(); // collide with enemy
        Base home = other.collider.GetComponent<Base>(); // collide with base

        if (attackTimer <= 0)
        {
            if (unit != null) unit.ChangeHealth(-damage);
            else if (home != null) home.ChangeHealth(-damage);

            attackTimer = attackCooldown;
        }
    }

    // Vector2: 2-dimensional vector
    void FixedUpdate()
    {
        if (!isLive) return;
        // move
        Vector2 nextVec = Vector2.left.normalized * speed * Time.fixedDeltaTime;
        rigid.MovePosition(rigid.position + nextVec);
        rigid.velocity = Vector2.zero;

        // attack
        attackTimer = Mathf.Clamp(attackTimer - Time.deltaTime, 0, attackCooldown);

        if (health > 0) {
            // anim.setTrigger("Hit);
            // play sfx sound of hit
        }
        else {
            coll.enabled = false;       // disable coliide2D
            rigid.simulated = false;    // disable rigid2D
            gameObject.SetActive(false);
        }
    }
    

    public void InitEnemy(SpawnData data) 
    {
        anim.runtimeAnimatorController = animCon[data.spriteType];
        speed = data.speed;
        health = data.health;
        maxHealth = data.health;
        attackCooldown = data.attackCooldown;
    }

    public void ChangeHealth(float amount) 
    {
      health = Mathf.Clamp(health + amount, 0, maxHealth);
    }
}
