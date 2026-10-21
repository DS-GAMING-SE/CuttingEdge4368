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
        public static Action<int> onEnemiesDefeated;
        private int numQueuedEnemiesDefeated;
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
        private bool enemySelectionDirty;
        private int nextWaveWithNewEnemy = 1;
        [SerializeField]
        private int baseSpawnCredits = 100;
        [SerializeField]
        private int spawnCreditIncreasePerWave = 15;
        [SerializeField]
        private int maxEnemiesPerWave = 5;
        private int spawnCredits;
        private List<EnemySpawnInfo> possibleSpawns = new List<EnemySpawnInfo>();
        private Stack<EnemySpawnInfo> queuedEnemySpawns = new Stack<EnemySpawnInfo>();
        private void OnEnable()
        {
            if (instance)
            {
                Destroy(gameObject);
                return;
            }
            instance = this;
        }
        private void OnDisable()
        {
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
        private void LateUpdate()
        {
            if (numQueuedEnemiesDefeated > 0)
            {
                SendEnemiesDefeatedEvent();
            }
        }
        public static void EnemyDefeated(EnemyController enemy)
        {
            if (!instance) return;

            instance.numQueuedEnemiesDefeated++;
            onEnemyDefeated?.Invoke(enemy);
        }
        private void SendEnemiesDefeatedEvent()
        {
            onEnemiesDefeated?.Invoke(numQueuedEnemiesDefeated);
            numQueuedEnemiesDefeated = 0;
            if (!InstanceTracker.Any<EnemyController>())
            {
                timeUntilEnemySpawn = timeBetweenWaves;
                enemiesActive = false;
            }
        }
        // I don't know if this works
        private EnemySpawnInfo GetWeightedSelection()
        {
            if (enemySelectionDirty)
            {
                nextWaveWithNewEnemy = int.MaxValue;
                totalWeight = 0f;
                possibleSpawns.Clear();
                foreach (var enemySpawnInfo in enemySpawns)
                {
                    if (wave >= enemySpawnInfo.minimumWave)
                    {
                        possibleSpawns.Add(enemySpawnInfo);
                        totalWeight += enemySpawnInfo.weight;
                    }
                    else
                    {
                        nextWaveWithNewEnemy = Math.Min(nextWaveWithNewEnemy, enemySpawnInfo.minimumWave);
                    }
                }
                enemySelectionDirty = false;
            }

            float index = UnityEngine.Random.value * totalWeight;

            float combinedWeight = 0f;
            for (int i = 0; i < possibleSpawns.Count; i++)
            {
                combinedWeight += possibleSpawns[i].weight;
                if (index < combinedWeight)
                {
                    return possibleSpawns[i];
                }
            }
            return enemySpawns[0];
        }
        private bool QueueEnemyWave()
        {
            if (wave == nextWaveWithNewEnemy) enemySelectionDirty = true;

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
