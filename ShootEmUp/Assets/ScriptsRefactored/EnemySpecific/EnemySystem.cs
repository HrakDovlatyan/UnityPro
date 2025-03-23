using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemySystem : MonoBehaviour
{
    [Header("Enemy Settings")]
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private int initialEnemyCount = 5;
    [SerializeField] private int maxEnemiesAlive = 10;
    [SerializeField] private float spawnInterval = 3f;

    [Header("Spawn & Attack Positions")]
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private Transform[] attackPositions;

    [SerializeField] private GameManager gameManager; 
    [SerializeField] private BulletSystem bulletSystem;

    private List<GameObject> activeEnemies = new List<GameObject>();
    private float nextSpawnTime;

    public event Action OnAllEnemiesDefeated;

    private void Start()
    {
        // Initial spawn
        for (int i = 0; i < initialEnemyCount; i++)
        {
            SpawnEnemy();
        }
        nextSpawnTime = Time.time + spawnInterval;
    }

    private void Update()
    {
        // Remove destroyed enemies
        activeEnemies.RemoveAll(enemy => enemy == null);

        // Check if all enemies are defeated
        if (activeEnemies.Count == 0)
        {
            OnAllEnemiesDefeated?.Invoke();
            return;
        }

        // Spawn new enemies
        if (Time.time >= nextSpawnTime && activeEnemies.Count < maxEnemiesAlive)
        {
            SpawnEnemy();
            nextSpawnTime = Time.time + spawnInterval;
        }
    }

    private void SpawnEnemy()
    {
        if (spawnPoints == null || spawnPoints.Length == 0 || attackPositions.Length == 0) return;

        // Pick random spawn point
        Transform spawnPoint = spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Length)];
        // Pick random attack position
        Transform attackPoint = attackPositions[UnityEngine.Random.Range(0, attackPositions.Length)];

        GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);

        // Pass the attack position to the EnemyController
        var enemyController = enemy.GetComponent<EnemyController>();
        if (enemyController != null)
        {
            enemyController.SetAttackPosition(attackPoint.position);
            enemyController.SetReferences(gameManager, bulletSystem);
        }

        activeEnemies.Add(enemy);
    }
}
