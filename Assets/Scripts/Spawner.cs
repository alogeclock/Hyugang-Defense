using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEditor.PackageManager;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public SpawnData[] spawnData;
    public Transform[] spawnPoint;

    int monsterType;
    public float spawnInterval = 20f;

    private void Awake()
    {
        spawnPoint = GetComponentsInChildren<Transform>();
        monsterType = spawnData.Length;
    }

    private void Start()
    {
        StartCoroutine(SpawnZombies()); // protocol that generates zombie
    }

    private IEnumerator SpawnZombies()
    {
        while (true)
        {
            Spawn();
            yield return new WaitForSeconds(spawnInterval); // generate zombie each spawnInterval
        }
    }

    public void Spawn()
    {
        // get enemy's prefab
        GameObject enemy = GameManager.instance.pool.Get(0);
        enemy.transform.position = spawnPoint[Random.Range(1, spawnPoint.Length)].position;
        enemy.GetComponent<Enemy>().InitEnemy(spawnData[Random.Range(0, monsterType)]);
    }
}

[System.Serializable]
public class SpawnData
{
    public int spriteType;
    public float speed;
    public int health;
    public float attackCooldown;
}