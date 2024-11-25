using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public UnitType unitType;
    public float health;
    public int maxHealth;
    public int level;
    public int price;

    public float earn;
    public float meleeDamage;
    public float Cooldown;
    float Timer;

    public bool isRanged; // range attack
    public bool isFarm;
    public bool isInCell;

    public GameObject bulletPrefab;
    public SpriteRenderer spriter;
    public BoxCollider2D coll;
    Rigidbody2D rigid;
    Animator anim;

    void Awake() {
        health = maxHealth;
        Timer = Cooldown;
        level = 1;
        isInCell = false;
        
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
            if (isRanged || !isInCell) return; // if range attack unit, collide(melee) damage is 0
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
        bullet.bulletDamage *= level; // level에 따라 데미지 배수로 증가
        bullet.Launch(Vector2.right, 200);
        // anim.SetTrigger("Launch");
    }

    public void Farm() {
        GameManager.instance.gold += (int)earn;
    }
    
    public void Upgrade() {
        // 레벨 증가
        // 체력 증가
        // 공격 속도 증가
        // price 값에 따라 업그레이드 가격 증가
        // 업그레이드 가격에 따라 price 값 증가 (판매 시 주는 돈 증가)
        // if (isFarm) earn 값 증가
    }

    private void OnMouseDown()
    {
        HandManager.instance.OnUnitClick(this);
    }
}