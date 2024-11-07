using System.Collections;
using System.Collections.Generic;
using System.Runtime;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    Rigidbody2D rigid;

    public float health;
    public int maxHealth;
    public int damage;

    public float speed;
    public float attackCooldown;
    float attackTimer;

    void Awake() 
    {
        rigid = GetComponent<Rigidbody2D>();
        health = maxHealth;
    }

    void OnEnable()
    {
        attackTimer = Mathf.Clamp(attackTimer - Time.deltaTime, 0, attackCooldown);
        if (health <= 0) Destroy(gameObject);
    }

    void OnCollisionStay2D(Collision2D other)
    {
        Unit unit = other.collider.GetComponent<Unit>(); // 적과 충돌
        Base home = other.collider.GetComponent<Base>(); // 기지과 충돌

        if (attackTimer <= 0)
        {
            if (unit != null) unit.ChangeHealth(-damage);
            else if (home != null) home.ChangeHealth(-damage);

            attackTimer = attackCooldown;
        }
    }

    // Vector2: 2차원 벡터
    void FixedUpdate()
    {
        // 이동
        Vector2 nextVec = Vector2.left.normalized * speed * Time.fixedDeltaTime;
        rigid.MovePosition(rigid.position + nextVec);
        rigid.velocity = Vector2.zero;

        // 공격
        attackTimer = Mathf.Clamp(attackTimer - Time.deltaTime, 0, attackCooldown);
        
        // 사망
        if (health <= 0) Destroy(gameObject);
    }

    public void ChangeHealth(float amount) 
    {
      health = Mathf.Clamp(health + amount, 0, maxHealth);
    }
}
