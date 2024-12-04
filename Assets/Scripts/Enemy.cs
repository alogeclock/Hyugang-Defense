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
    public AudioClip bossDeathSound; // Boss死亡音效
    public AudioClip bossAttackSound; // Boss攻击音效
    private AudioSource audioSource;

    public float referenceWidth = 1920f; // 기준 화면 높이 (픽셀)

    void Start() {
        SpeedChangeToCamera();
    }

    void SpeedChangeToCamera()
    {
        float currentWidth = Screen.width;
        float speedRatio = Mathf.Min(currentWidth / referenceWidth, 1f);
        speed *= speedRatio;
    }

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
        spriter.color = new Color(1f, 1f, 1f, 1f);
    }

    void OnCollisionStay2D(Collision2D other)
    {
        if (!isLive) return;
        Unit unit = other.collider.GetComponent<Unit>();
        Base home = other.collider.GetComponent<Base>();

        if (attackTimer <= 0)
        {
            if (unit != null || home != null) {
                anim.SetTrigger("Attack");
                if (unit != null) unit.ChangeHealth(-damage);
                else if (home != null) home.ChangeHealth(-damage);

                // 如果是Boss，则播放Boss的攻击音效
                if (isBoss && bossAttackSound != null)
                    audioSource.PlayOneShot(bossAttackSound);
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

        // 对Boss播放不同的死亡音效
        if (isBoss && bossDeathSound != null && audioSource != null)
            audioSource.PlayOneShot(bossDeathSound); // Boss死亡音效
        else if (deathSound != null && audioSource != null)
            audioSource.PlayOneShot(deathSound); // 普通敌人死亡音效
        yield return new WaitForSeconds(deathSound.length); // 等待音效播放完

        Color spriteColor = spriter.color;
        float fadeDuration = 0.5f; // fade out duration
        float timer = 0f;

        // Fade out effect
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, timer / fadeDuration);
            spriteColor.a = alpha;
            spriter.color = spriteColor;
            yield return null;
        }

        GameManager.instance.score += maxHealth;
        coll.enabled = false;       // Disable collider
        rigid.simulated = false;    // Disable rigidbody

        // 如果是Boss，触发Boss死亡后的一些事件
        if (isBoss)
        {
            GameManager.instance.Win();
            // 你可以在这里添加其他Boss死亡后的一些行为，例如掉落奖励、召唤其他敌人等
            // 示例：
            // GameManager.instance.SpawnMinions(); // 假设GameManager有SpawnMinions方法
        }

        gameObject.SetActive(false); // Deactivate the game object
    }

    void FixedUpdate()
    {
        if (!isLive || anim.GetCurrentAnimatorStateInfo(0).IsName("Hit")) return;

        // Move the enemy
        Vector2 nextVec = Vector2.left.normalized * speed * Time.fixedDeltaTime;
        rigid.MovePosition(rigid.position + nextVec);
        rigid.velocity = Vector2.zero;
        anim.SetFloat("Speed", speed);

        // Handle attack cooldown
        attackTimer = Mathf.Clamp(attackTimer - Time.deltaTime, 0, attackCooldown);

        if (health <= 0)
        {
            StartCoroutine(Die());
        }
    }

    IEnumerator Hit()
    {
        Debug.Log(gameObject + " is Attacked (in Hit())");
        yield return new WaitForSeconds(0.3f);
        Color spriteColor = spriter.color;
        Color hitColor = new Color(1f, 0.8f, 0.8f, 1f);

        float fadeDuration = 0.2f; // fade duration
        float timer = 0f;

        // Transition to hit color
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            spriter.color = Color.Lerp(spriteColor, hitColor, timer / fadeDuration);
            yield return null;
        }

        // Restore original color
        timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            spriter.color = Color.Lerp(hitColor, spriteColor, timer / fadeDuration);
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

        if (isBoss) {
            healthBar.SetActive(true);
            rigid.mass = 5;
        }
        else healthBar.SetActive(false);
    }

    public void ChangeHealth(float amount)
    {
        Debug.Log("enemy is attacked");
        health = Mathf.Clamp(health + amount, 0, maxHealth);
        if (amount < 0)
        {
            audioSource.PlayOneShot(hitSound); // 播放音效
            StartCoroutine(Hit()); // 붉은色으로 깜빡임
        }
        fill.fillAmount = health / maxHealth;
    }
}
