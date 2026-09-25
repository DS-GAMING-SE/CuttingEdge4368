using DSGameUtils.Pools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class EffectManager
{
    public static GameObject SimpleEffect(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        GameObject effect = DynamicGameObjectPool.CreateOrGetPooledObject(prefab);
        effect.transform.SetParent(null);
        effect.transform.SetPositionAndRotation(position, rotation);
        return effect;
    }
}
