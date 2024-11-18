using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.PackageManager;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public static Spawner instance;
    public SpawnData[] spawnData;
    public Transform[] spawnPoint;

    private float spawnInterval;
    private float timer = 0.0f;

    private void Awake()
    {
        instance = this;
        spawnPoint = GetComponentsInChildren<Transform>();
    }

    private void FixedUpdate() 
    {
        timer += Time.deltaTime;
        if (timer < spawnInterval) return;
        
        spawnInterval = GameManager.instance.spawnInterval;
        Spawn();
        timer = 0.0f;
    }
    
    public void Spawn()
    {
        // get enemy's prefab
        GameObject enemy = GameManager.instance.pool.Get(0);
        
        int line = Random.Range(1, spawnPoint.Length);
        int monsterType = Random.Range(0, GameManager.instance.monsterLevel);

        enemy.transform.position = spawnPoint[line].position;
        enemy.GetComponent<Enemy>().InitEnemy(spawnData[monsterType], line);
    }
}

[System.Serializable]
public class SpawnData
{
    public int spriteType;
    public float speed;
    public int health;
    public int damage;
    public float attackCooldown;
    public float size;
}