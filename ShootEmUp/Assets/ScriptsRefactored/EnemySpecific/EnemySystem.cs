using ShootEmUp.Controllers;
using ShootEmUp.Systems;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace ShootEmUp.Systems
{
    public class EnemySystem : MonoBehaviour
    {

        public event Action OnAllEnemiesDefeated;


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

        private readonly List<GameObject> activeEnemies = new List<GameObject>();
        private float nextSpawnTime;


        private void Start()
        {
            for (int i = 0; i < initialEnemyCount; i++)
            {
                SpawnEnemy();
            }
            nextSpawnTime = Time.time + spawnInterval;
        }

        private void Update()
        {
            activeEnemies.RemoveAll(enemy => enemy == null);

            if (activeEnemies.Count == 0)
            {
                OnAllEnemiesDefeated?.Invoke();
                return;
            }

            if (Time.time >= nextSpawnTime && activeEnemies.Count < maxEnemiesAlive)
            {
                SpawnEnemy();
                nextSpawnTime = Time.time + spawnInterval;
            }
        }

        private void SpawnEnemy()
        {
            if (spawnPoints == null || spawnPoints.Length == 0 || attackPositions.Length == 0) return;

            Transform spawnPoint = spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Length)];

            Transform attackPoint = attackPositions[UnityEngine.Random.Range(0, attackPositions.Length)];

            GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);

            var enemyController = enemy.GetComponent<EnemyController>();
            if (enemyController != null)
            {
                enemyController.SetAttackPosition(attackPoint.position);
                enemyController.SetReferences(bulletSystem);
            }

            activeEnemies.Add(enemy);
        }
    }
}