using DSGameUtils.Pools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class EffectManager
{
    private static Dictionary<GameObject, GameObjectPool> effectPrefabToPool = new Dictionary<GameObject, GameObjectPool>();
    [RuntimeInitializeOnLoadMethod]
    private static void Initialize()
    {
        SceneManager.activeSceneChanged += (scene, scene2) => { effectPrefabToPool.Clear(); };
    }
    public static GameObject SimpleEffect(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        GameObject effect = CreateOrGetPooledEffect(prefab);
        effect.transform.SetParent(null);
        effect.transform.SetPositionAndRotation(position, rotation);
        return effect;
    }
    public static GameObject CreateOrGetPooledEffect(GameObject prefab)
    {
        if (effectPrefabToPool.TryGetValue(prefab, out var pool))
        {
            return pool.Get();
        }
        else
        {
            GameObjectPool newPool = new GameObjectPool(prefab);
            effectPrefabToPool.Add(prefab, newPool);
            return newPool.Get();
        }
    }
}
