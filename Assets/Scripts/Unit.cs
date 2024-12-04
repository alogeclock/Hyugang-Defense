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
    float hpTimer;

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
        hpTimer = 4f;
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
        hpTimer = Mathf.Clamp(hpTimer - Time.deltaTime, 0, 4f);

        if (Timer <= 0)
        {
            if (isRanged) RangeAttack();
            if (isFarm) Farm();
            Timer = Cooldown;
        }
        
        if (hpTimer <= 0) {
            ChangeHealth(Mathf.Floor(maxHealth * 0.01f));
            hpTimer = 2f;
        }
        if (health <= 0) Destroy(gameObject); // dead
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
            if (hitSound != null && audioSource != null) audioSource.PlayOneShot(hitSound);
            StartCoroutine(Hit()); // 하얀색으로 깜빡임
        }
        health = Mathf.Clamp(health + amount, 0, maxHealth);
    }

    IEnumerator Hit() {
        // Debug.Log(gameObject + "is Attacked by Zombie (in Hit())");
        Color spriteColor = spriter.color;
        Color hitColor = new Color(1f, 0.8f, 0.8f, 1f);

        float fadeDuration = 0.2f; // 페이드 아웃 지속 시간
        float timer = 0f;

        // 하얀색으로 전환
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            spriter.color = Color.Lerp(spriteColor, hitColor, timer / fadeDuration);
            Debug.Log("Original Color: " + spriter.color);
            yield return null;
        }

        // 원래 색으로 복구
        timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            spriter.color = Color.Lerp(hitColor, spriteColor, timer / fadeDuration);
            Debug.Log("Original Color: " + spriter.color);
            yield return null;
        }

        spriter.color = spriteColor;
    }

    public void RangeAttack()
    {
        Vector2 bulletHeight = rigid.position + Vector2.up * (spriter.bounds.size.y / 2);
        GameObject bulletObject = Instantiate(bulletPrefab, bulletHeight, Quaternion.identity);
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
