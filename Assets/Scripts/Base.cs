using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base : MonoBehaviour
{
    public float health;
    public float maxHealth;
    public bool isFence;

    void Awake() {
        health = maxHealth;
    }

    void FixedUpdate() {
        if (health <= 0) {
            GameManager.instance.Lose();
            // if (!isFence) GameOver() output function
        }
    }

    public void ChangeHealth(float amount)
    {
        health = Mathf.Clamp(health + amount, 0, maxHealth);
    }
}
