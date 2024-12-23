using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base : MonoBehaviour
{
    public float health;
    public float maxHealth;
    public bool isFence;

    public AudioClip hitSound;
    private AudioSource audioSource;

    void Awake() {
        health = maxHealth;
        audioSource = GetComponent<AudioSource>();
    }

    void FixedUpdate() {
        if (health <= 0) {
            GameManager.instance.Lose();

            // **************************************************
            // [기지가 무너지는 소리 넣을 곳]
            // sound of the base being destroyed
            // **************************************************

            // if (!isFence) GameOver() output function
        }
    }

    public void ChangeHealth(float amount)
    {
        
        if (amount < 0) if (hitSound != null && audioSource != null) audioSource.PlayOneShot(hitSound);
        health = Mathf.Clamp(health + amount, 0, maxHealth);
    }
}
