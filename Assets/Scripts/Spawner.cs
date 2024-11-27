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

    private void Start()
    {
        spawnInterval = GameManager.instance.spawnInterval;
    }

    private void FixedUpdate() 
    {
        timer += Time.deltaTime;
        if (timer < spawnInterval) return;
        
        spawnInterval = GameManager.instance.spawnInterval;
        Spawn();
        timer = 0.0f;
    }

    public void StopSpawning()
    {
        timer = 0.0f;
        spawnInterval = 1000000.0f;
    }
    
    public void Spawn()
    {
        // get enemy's prefab
        GameObject enemy = GameManager.instance.pool.Get(0);
        enemy.tag = "Enemy";
        
        int line = Random.Range(1, spawnPoint.Length);
        int level = Mathf.Min(GameManager.instance.monsterLevel, spawnData.Length);
        int monsterType = Random.Range(0, level); // 33.3%

        enemy.transform.position = spawnPoint[line].position;
        enemy.GetComponent<Enemy>().InitEnemy(spawnData[monsterType], line);
    }

    public void SpawnBoss(int monsterType, int line)
    {
        GameObject enemy = GameManager.instance.pool.Get(0);
        enemy.tag = "Enemy";
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
    public bool isBoss;
}