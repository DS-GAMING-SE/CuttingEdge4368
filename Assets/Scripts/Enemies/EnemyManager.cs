using DSGameUtils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CuttingEdge.Enemies
{
    public class EnemyManager : MonoBehaviour
    {
        public static EnemyManager instance;
        public static Action<EnemyController> onEnemyDefeated; 
        public bool spawningEnemies = false;
        [NonSerialized]
        public bool enemiesActive = false;
        private int wave;
        [SerializeField]
        private float timeUntilEnemySpawn = 3f;
        [SerializeField]
        private float timeBetweenWaves = 2f;
        [SerializeField]
        private float timeBetweenEnemySpawns = 0.3f;
        [SerializeField]
        private EnemySpawnInfo[] enemySpawns;
        private float totalWeight = -1f;
        [SerializeField]
        private int baseSpawnCredits = 100;
        [SerializeField]
        private int spawnCreditIncreasePerWave = 15;
        [SerializeField]
        private int maxEnemiesPerWave = 5;
        private int spawnCredits;
        private Stack<EnemySpawnInfo> queuedEnemySpawns = new Stack<EnemySpawnInfo>();
        private void OnEnable()
        {
            if (instance)
            {
                Destroy(gameObject);
                return;
            }
            instance = this;
            onEnemyDefeated += EnemyDefeated;
        }
        private void OnDisable()
        {
            onEnemyDefeated -= EnemyDefeated;
            if (instance == this)
            {
                instance = null;
            }
        }
        public static EnemyController SpawnEnemy(GameObject prefab, Vector3 position)
        {
            if (!prefab) return null;

            GameObject enemy = GameObject.Instantiate(prefab, Plane.enemyPlane.transform);
            enemy.transform.position = position;

            return enemy.GetComponent<EnemyController>();
        }
        private void FixedUpdate()
        {
            if (!spawningEnemies || (enemiesActive && queuedEnemySpawns.Count == 0)) return;

            timeUntilEnemySpawn -= Time.fixedDeltaTime;
            if (timeUntilEnemySpawn <= 0)
            {
                if (queuedEnemySpawns.Count == 0)
                {
                    wave++;
                    spawnCredits += baseSpawnCredits + (spawnCreditIncreasePerWave * (wave - 1));
                    if (QueueEnemyWave())
                    {
                        enemiesActive = true;
                    }
                }
                if (queuedEnemySpawns.Count > 0)
                {
                    SpawnEnemy(queuedEnemySpawns.Pop().spawnCard.prefab, Plane.enemyPlane.GetRandomWorldPositionOnPlane());
                }
                timeUntilEnemySpawn += timeBetweenEnemySpawns;
            }
        }
        private void EnemyDefeated(EnemyController enemy)
        {
            if (!InstanceTracker.Any<EnemyController>())
            {
                timeUntilEnemySpawn = timeBetweenWaves;
                enemiesActive = false;
            }
        }
        // I don't know if this works
        private EnemySpawnInfo GetWeightedSelection()
        {
            if (totalWeight < 0)
            {
                foreach (var enemySpawnInfo in enemySpawns)
                {
                    totalWeight += enemySpawnInfo.weight;
                }
            }

            float index = UnityEngine.Random.value * totalWeight;

            float combinedWeight = 0f;
            for (int i = 0; i < enemySpawns.Length; i++)
            {
                combinedWeight += enemySpawns[i].weight;
                if (index < combinedWeight)
                {
                    return enemySpawns[i];
                }
            }
            return enemySpawns[0];
        }
        private bool QueueEnemyWave()
        {
            bool spawned = false;
            for (int i = 0; i < maxEnemiesPerWave; i ++)
            {
                EnemySpawnInfo enemySpawnInfo = GetWeightedSelection();
                if (spawnCredits >= enemySpawnInfo.spawnCard.spawnCost)
                {
                    queuedEnemySpawns.Push(enemySpawnInfo);
                    spawnCredits -= enemySpawnInfo.spawnCard.spawnCost;
                    spawned = true;
                }
            }
            return spawned;
        }
        [Serializable]
        public struct EnemySpawnInfo
        {
            public SpawnCard spawnCard;
            public float weight;
            public int minimumWave;
        }
    }
}
