using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            }
        }
    }

    void Update() 
    {
        if (transform.position.magnitude > 50.0f) Destroy(gameObject);
    }
}
