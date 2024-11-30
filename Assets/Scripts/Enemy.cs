using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Analytics;

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
    public GameObject healthBar;
    public Image fill;

    Animator anim;
    SpriteRenderer spriter;
    BoxCollider2D coll;
    Rigidbody2D rigid;

    public AudioClip hitSound; // 攻击音效
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
        spriter.color = new Color (1f, 1f, 1f, 1f);
    }

    void OnCollisionStay2D(Collision2D other)
    {
        if (!isLive) return;
        Unit unit = other.collider.GetComponent<Unit>();
        Base home = other.collider.GetComponent<Base>();

        if (attackTimer <= 0)
        {
            if (unit != null || home != null) {
                if (unit != null) unit.ChangeHealth(-damage);
                else if (home != null) home.ChangeHealth(-damage);
                anim.SetTrigger("Attack");
            }
            else {
                attackTimer = 0;
                return;
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

        Color spriteColor = spriter.color;
        float fadeDuration = 0.5f; // 페이드 아웃 지속 시간
        float timer = 0f;

        // 페이드 아웃 루프
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, timer / fadeDuration);
            spriteColor.a = alpha;
            spriter.color = spriteColor;
            yield return null;
        }

        GameManager.instance.score += maxHealth;
        coll.enabled = false;       // 콜라이더 비활성화
        rigid.simulated = false;    // Rigidbody 비활성화

        if (isBoss) GameManager.instance.Win();
        gameObject.SetActive(false); // 게임 오브젝트 비활성화
    }

    void FixedUpdate()
    {
        if (!isLive || anim.GetCurrentAnimatorStateInfo(0).IsName("Hit")) return;

        // move
        Vector2 nextVec = Vector2.left.normalized * speed * Time.fixedDeltaTime;
        rigid.MovePosition(rigid.position + nextVec);
        rigid.velocity = Vector2.zero;
        anim.SetFloat("Speed", speed);

        // attack
        attackTimer = Mathf.Clamp(attackTimer - Time.deltaTime, 0, attackCooldown);

        if (health > 0) {}
        else StartCoroutine(Die());
    }

    
    IEnumerator Hit() {
        Debug.Log(gameObject + "is Attacked by Zombie (in Hit())");
        yield return new WaitForSeconds(0.3f);
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

    public void InitEnemy(SpawnData data, int line) 
    {
        Debug.Log("Init Enemy Size is " + data.size);
        spriter.sortingOrder = line;
        transform.localScale = new Vector2(data.size, data.size);

        int round = Mathf.Min(GameManager.instance.round - 1, animCon.Length - 1);
        Debug.Log("round number in InitEnemy() is " + round);

        anim.runtimeAnimatorController = animCon[data.spriteType];
        speed = data.speed;
        damage = data.damage;
        health = data.health[round];
        maxHealth = data.health[round];
        attackCooldown = data.attackCooldown;
        isBoss = data.isBoss;
        
        if (isBoss) healthBar.SetActive(true);
        else healthBar.SetActive(false);
    }

    public void ChangeHealth(float amount) 
    {
        Debug.Log("enemy is attacked");
        health = Mathf.Clamp(health + amount, 0, maxHealth);
        if (amount < 0) {
            audioSource.PlayOneShot(hitSound); // 播放音效
            StartCoroutine(Hit()); // 붉은색으로 깜빡임
        }
        fill.fillAmount = health / maxHealth;
    }
}