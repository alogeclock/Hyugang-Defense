using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public UnitType unitType;
    public float health;
    public int maxHealth;    

    public float earn;
    public float meleeDamage;
    public float Cooldown;
    float Timer;

    public bool isRanged; // range attack
    public bool isFarm;

    public GameObject bulletPrefab;
    public SpriteRenderer spriter;
    public BoxCollider2D coll;
    Rigidbody2D rigid;
    Animator anim;

    void Awake() {
        health = maxHealth;
        Timer = Cooldown;
        
        rigid = GetComponent<Rigidbody2D>();
        spriter = GetComponent<SpriteRenderer>();
        coll = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        // attack
        Timer = Mathf.Clamp(Timer - Time.deltaTime, 0, Cooldown);

        if (Timer <= 0) {
            if (isRanged) RangeAttack();
            if (isFarm) Farm();
            Timer = Cooldown;
        }

        // dead
        if (health <= 0) Destroy(gameObject);
    }

    void OnCollisionStay2D(Collision2D other) 
    {
        Enemy enemy = other.collider.GetComponent<Enemy>(); // collide with enemy

        if (Timer <= 0 && enemy != null) {
            if (isRanged) return; // if range attack unit, collide(melee) damage is 0
            enemy.ChangeHealth(-meleeDamage);
            Timer = Cooldown;
        }
    }

    public void ChangeHealth(float amount)
    {
        health = Mathf.Clamp(health + amount, 0, maxHealth);
    }

    public void RangeAttack() {
        // Debug.Log("Range Attack!");
        GameObject bulletObject = Instantiate(bulletPrefab, rigid.position + Vector2.up * 1.2f, Quaternion.identity);
        Bullet bullet = bulletObject.GetComponent<Bullet>();
        bullet.Launch(Vector2.right, 200);
        // anim.SetTrigger("Launch");
    }

    public void Farm() {
        GameManager.instance.gold += (int)earn;
    }
}