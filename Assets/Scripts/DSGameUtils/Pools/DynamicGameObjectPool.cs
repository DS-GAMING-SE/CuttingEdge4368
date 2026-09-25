using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DSGameUtils.Pools
{
    public static class DynamicGameObjectPool
    {
        private static Dictionary<GameObject, GameObjectPool> prefabToPool = new Dictionary<GameObject, GameObjectPool>();
        [RuntimeInitializeOnLoadMethod]
        private static void Initialize()
        {
            SceneManager.activeSceneChanged += (scene, scene2) =>
            {
                foreach (var pool in prefabToPool.Values)
                {
                    pool.Clear();
                }
                prefabToPool.Clear();
            };
        }
        public static GameObject CreateOrGetPooledObject(GameObject prefab, bool deactivated = false)
        {
            if (prefabToPool.TryGetValue(prefab, out var pool))
            {
                return deactivated ? pool.GetDeactivated() : pool.Get();
            }
            else
            {
                GameObjectPool newPool = new GameObjectPool(prefab);
                prefabToPool.Add(prefab, newPool);
                return deactivated ?  newPool.GetDeactivated() : newPool.Get();
            }
        }
    }
}
