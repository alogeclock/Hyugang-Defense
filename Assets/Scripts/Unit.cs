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
    public float damage;
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

    public AudioClip attackSound; // 攻击音效
    public AudioClip hitSound;    // 受击音效
    private AudioSource audioSource;

    void Awake()
    {
        health = maxHealth;
        Timer = Cooldown;
        level = 1;
        isInCell = false;

        rigid = GetComponent<Rigidbody2D>();
        spriter = GetComponent<SpriteRenderer>();
        coll = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();

        // 添加或获取 AudioSource 组件
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    void FixedUpdate()
    {
        // attack
        Timer = Mathf.Clamp(Timer - Time.deltaTime, 0, Cooldown);

        if (Timer <= 0)
        {
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

        if (Timer <= 0 && enemy != null)
        {
            if (isRanged || !isInCell) return; // if range attack unit, collide(melee) damage is 0
            enemy.ChangeHealth(-damage);
            Timer = Cooldown;
        }
    }

    public void ChangeHealth(float amount)
    {
        if (amount < 0) // 播放受击音效
        {
            if (hitSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(hitSound);
            }
        }

        health = Mathf.Clamp(health + amount, 0, maxHealth);
    }

    public void RangeAttack()
    {
        GameObject bulletObject = Instantiate(bulletPrefab, rigid.position + Vector2.up * 1.2f, Quaternion.identity);
        Bullet bullet = bulletObject.GetComponent<Bullet>();
        bullet.bulletDamage = (int)damage;
        bullet.Launch(Vector2.right, 200);

        // 播放攻击音效
        if (attackSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(attackSound);
        }
    }

    public void Farm()
    {
        GameManager.instance.gold += (int)earn;
    }

    private void OnMouseDown()
    {
        HandManager.instance.OnUnitClick(this);
    }
}
