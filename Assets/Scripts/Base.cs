using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base : MonoBehaviour
{
    public int health;
    public int maxHealth;

    void Awake() {
        health = maxHealth;
    }
}
