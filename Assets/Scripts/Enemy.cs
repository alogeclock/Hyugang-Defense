using System.Collections;
using System.Collections.Generic;
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
    public bool isBoss;

    Animator anim;
    SpriteRenderer spriter;
    BoxCollider2D coll;
    Rigidbody2D rigid;

    public AudioClip attackSound; // 攻击音效
    public AudioClip deathSound;  // 死亡音效
    private AudioSource audioSource;

    void Awake() 
    {
        rigid = GetComponent<Rigidbody2D>();
        spriter = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        coll = GetComponent<BoxCollider2D>();
        audioSource = GetComponent<AudioSource>(); // 获取 AudioSource 组件
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
        Unit unit = other.collider.GetComponent<Unit>(); // 碰撞到单位
        Base home = other.collider.GetComponent<Base>(); // 碰撞到基地

        if (attackTimer <= 0)
        {
            if (unit != null) unit.ChangeHealth(-damage);
            else if (home != null) home.ChangeHealth(-damage);

            // 播放敌人攻击音效
            if (attackSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(attackSound); // 播放音效
            }

            attackTimer = attackCooldown;
        }
    }

    IEnumerator Die()
    {
        Debug.Log("enemy is died");
        isLive = false;

        // 사망 사운드 재생
        if (deathSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(deathSound);
            yield return new WaitForSeconds(deathSound.length); // 사운드 길이만큼 대기
        }

        GameManager.instance.score += maxHealth;
        coll.enabled = false;       // 콜라이더 비활성화
        rigid.simulated = false;    // Rigidbody 비활성화

        if (isBoss) GameManager.instance.Win();
        gameObject.SetActive(false); // 게임 오브젝트 비활성화
    }

    void FixedUpdate()
    {
        if (!isLive) return;

        // move
        Vector2 nextVec = Vector2.left.normalized * speed * Time.fixedDeltaTime;
        rigid.MovePosition(rigid.position + nextVec);
        rigid.velocity = Vector2.zero;
        anim.SetFloat("Speed", speed);

        // attack
        attackTimer = Mathf.Clamp(attackTimer - Time.deltaTime, 0, attackCooldown);

        if (health > 0)
        {
            // 播放受击音效（可以选择性播放）
            // anim.SetTrigger("Hit");
        }
        else StartCoroutine(Die());
    }

    public void InitEnemy(SpawnData data, int line) 
    {
        Debug.Log("Init Enemy Size is " + data.size);
        spriter.sortingOrder = line;
        transform.localScale = new Vector2(data.size, data.size);

        anim.runtimeAnimatorController = animCon[data.spriteType];
        speed = data.speed;
        damage = data.damage;
        health = data.health;
        maxHealth = data.health;
        attackCooldown = data.attackCooldown;
        isBoss = data.isBoss;
    }

    public void ChangeHealth(float amount) 
    {
        Debug.Log("enemy is attacked");
        health = Mathf.Clamp(health + amount, 0, maxHealth);
    }
}