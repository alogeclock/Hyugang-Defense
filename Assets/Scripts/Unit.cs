using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public float health;
    public int maxHealth;    

    public float damage;
    public float attackCooldown;
    float attackTimer;

    public bool isRanged; // 원거리 공격

    public GameObject bulletPrefab;
    Rigidbody2D rigid;
    Animator anim;

    void Awake() {
        health = maxHealth;
        attackTimer = attackCooldown;
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        // 공격
        attackTimer = Mathf.Clamp(attackTimer - Time.deltaTime, 0, attackCooldown);
        if (isRanged && attackTimer <= 0) {
            RangeAttack();
            attackTimer = attackCooldown;
        }

        // 사망
        if (health <= 0) Destroy(gameObject);
    }

    void OnCollisionStay2D(Collision2D other) 
    {
        Enemy enemy = other.collider.GetComponent<Enemy>(); // 적과 충돌

        if (attackTimer <= 0 && enemy != null) {
            if (isRanged) return; // 원거리 공격 유닛이라면 충돌 데미지 0
            enemy.ChangeHealth(-damage);
            attackTimer = attackCooldown;
        }
    }

    public void ChangeHealth(float amount)
    {
        health = Mathf.Clamp(health + amount, 0, maxHealth);
    }

    public void RangeAttack() {
        // Debug.Log("Range Attack!");
        GameObject bulletObject = Instantiate(bulletPrefab, rigid.position + Vector2.up * 0.5f, Quaternion.identity);

        Bullet bullet = bulletObject.GetComponent<Bullet>();
        bullet.Launch(Vector2.right, 200);
        // anim.SetTrigger("Launch");
    }
}