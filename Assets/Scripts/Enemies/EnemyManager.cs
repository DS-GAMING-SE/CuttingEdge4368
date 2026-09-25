using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CuttingEdge.Enemies
{
    public static class EnemyManager
    {
        public static Action<EnemyController> onEnemyDefeated; 
        public static List<EnemyController> activeEnemies = new List<EnemyController>();
        public static EnemyController SpawnEnemy(GameObject prefab, Vector3 position)
        {
            if (!prefab) return null;

            GameObject enemy = GameObject.Instantiate(prefab, Plane.enemyPlane.transform);
            enemy.transform.position = position;

            return null;
        }
        [RuntimeInitializeOnLoadMethod]
        private static void Initialize()
        {
            CuttingEdgeApplication.onFixedUpdate += FixedUpdate;
        }

        private static float timeUntilEnemySpawn;
        private static void FixedUpdate()
        {
            timeUntilEnemySpawn -= Time.fixedDeltaTime;
            if (timeUntilEnemySpawn <= 0)
            {
                TestSpawnEnemies();
                timeUntilEnemySpawn = 7f;
            }
        }

        private static void TestSpawnEnemies()
        {
            SpawnEnemy(Resources.Load<GameObject>("TestMovingCube"), Plane.enemyPlane.GetRandomWorldPositionOnPlane());
            SpawnEnemy(Resources.Load<GameObject>("TestMovingCube"), Plane.enemyPlane.GetRandomWorldPositionOnPlane());
            SpawnEnemy(Resources.Load<GameObject>("TestMovingCube"), Plane.enemyPlane.GetRandomWorldPositionOnPlane());
            SpawnEnemy(Resources.Load<GameObject>("TestMovingCube"), Plane.enemyPlane.GetRandomWorldPositionOnPlane());
        }
    }
}
