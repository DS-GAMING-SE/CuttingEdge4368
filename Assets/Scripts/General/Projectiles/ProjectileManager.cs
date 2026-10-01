using CuttingEdge.Enemies;
using DSGameUtils.Pools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CuttingEdge
{
    public static class ProjectileManager
    {
        public static ProjectileController FireProjectile(GameObject prefab, Vector3 spawnPosition, GameObject owner, Transform target)
        {
            return FireProjectile(prefab, spawnPosition, Quaternion.LookRotation(target.position - spawnPosition), owner, target);
        }
        public static ProjectileController FireProjectile(GameObject prefab, Vector3 spawnPosition, Quaternion rotation, GameObject owner, Transform target)
        {
            if (!prefab) return null;
            GameObject projectile = DynamicGameObjectPool.CreateOrGetPooledObject(prefab, true);
            if (projectile.TryGetComponent<ProjectileController>(out var projectileController))
            {
                projectileController.owner = owner;
                projectileController.target = target;
                projectile.transform.SetPositionAndRotation(spawnPosition, rotation);
                projectileController.SetTargetToPlayer();

                projectile.SetActive(true);
                return projectileController;
            }
            return null;
        }
    }
}
