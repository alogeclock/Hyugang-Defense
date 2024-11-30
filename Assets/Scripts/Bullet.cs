using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UIElements;

public class Bullet : MonoBehaviour
{
    public int bulletDamage;
    Rigidbody2D rigid;
    public bool isCollide;


    void Awake() 
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    public void Launch(Vector2 dir, float force) {
        rigid.AddForce(dir * force);
    }

    void OnCollisionStay2D(Collision2D other) {
        if (isCollide) return;
        isCollide = true;

        if (this.gameObject.CompareTag("Player")) { // 아군 소유의 탄막일 경우
            Enemy enemy = other.collider.GetComponent<Enemy>();
            if (enemy != null)
            {
                Destroy(gameObject);
                enemy.ChangeHealth(-bulletDamage);
                Debug.Log(this.gameObject.name + " is Attacked " + enemy.gameObject.name);
                // **************************************************
                // [적 몬스터가 공격당하는 소리 넣을 곳]
                // place for sound of enemy monster being attacked
                // **************************************************
            }
        }
    }

    void Update() 
    {
        transform.Rotate(Vector3.back * 150f * Time.deltaTime);
        if (transform.position.magnitude > 20.0f) Destroy(gameObject);
    }
}
