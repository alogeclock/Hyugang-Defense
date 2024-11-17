using System.Collections;
using System.Collections.Generic;
using UnityEngine;


enum ZombieState
{
    Move,
    Eat,
    Die,
    Pause
}
public class Zombie : MonoBehaviour
{
    ZombieState zombieState = ZombieState.Move;
    private Rigidbody2D rgd;
    public float moveSpeed = 2;
    private Animator anim;

    public int atkValue=30;
    public float atkDuration = 2;
    private float atkTimer = 0;

    private Plant currentEatPlant;

    public int HP = 100;
    private int currentHP;
    public GameObject zombieHeadPrefab;

    private bool haveHead = true;

    // Start is called before the first frame update
    void Start()
    {
        rgd = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        currentHP = HP;
    }

    // Update is called once per frame
    void Update()
    {
        switch (zombieState)
        {
            case ZombieState.Move:
                MoveUpdate();
                break;
            case ZombieState.Eat:
                EatUpdate();
                break;
            case ZombieState.Die:
                break;
            default:
                break;
        }
    }

    void MoveUpdate()
    {
        rgd.MovePosition(rgd.position + Vector2.left * moveSpeed * Time.deltaTime);
    }
    void EatUpdate()
    {
        atkTimer += Time.deltaTime;
        if (atkTimer > atkDuration && currentEatPlant!=null)
        {
            AudioManager.Instance.PlayClip(Config.eat);
            currentEatPlant.TakeDamage(atkValue);
            atkTimer = 0;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Plant")
        {
            anim.SetBool("IsAttacking", true);
            TransitionToEat();
            currentEatPlant = collision.GetComponent<Plant>();
        }else if (collision.tag == "House")
        {
            ZombieGameManager.Instance.GameEndFail();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Plant")
        {
            anim.SetBool("IsAttacking", false);
            zombieState = ZombieState.Move;
            currentEatPlant = null;
        }
    }

    void TransitionToEat()
    {
        zombieState = ZombieState.Eat;
        atkTimer = 0;
    }
    public void TransitionToPause()
    {
        zombieState = ZombieState.Pause;
        anim.enabled = false;
    }
    public void TakeDamage(int damage)
    {
        if (currentHP <= 0) return;

        this.currentHP -= damage;
        if(currentHP <= 0)
        {
            currentHP = -1;
            Dead();
        }
        float hpPercent = currentHP*1f / HP;
        anim.SetFloat("HPPercent", hpPercent);
        if (hpPercent < .5f &&haveHead )
        {
            haveHead = false;
            GameObject go= GameObject.Instantiate(zombieHeadPrefab, transform.position, Quaternion.identity);
            Destroy(go, 2);
        }
    }

    private void Dead()
    {
        if (zombieState == ZombieState.Die) return;

        zombieState = ZombieState.Die;
        GetComponent<Collider2D>().enabled = false;
        ZombieManager.Instance.RemoveZombie(this);

        Destroy(this.gameObject, 2);
    }
}
