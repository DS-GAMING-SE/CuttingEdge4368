using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DSGameUtils.Pools
{
    public class GameObjectPool : Pool<GameObject>
    {
        public GameObject prefab;
        public override GameObject Get()
        {
            GameObject pooledObject = base.Get();
            pooledObject.SetActive(true);
            return pooledObject;
        }
        protected override GameObject CreateNewPooledObject()
        {
            GameObject pooledObject = GameObject.Instantiate(prefab);
            pooledObject.hideFlags = HideFlags.DontSaveInEditor;
            foreach (Transform child in pooledObject.transform)
            {
                child.hideFlags = HideFlags.DontSaveInEditor;
            }
            pooledObject.EnsureComponent<PooledGameObject>().pool = this;
            return pooledObject;
        }
        protected override void ReturnObject(GameObject pooledObject)
        {
            pooledObject.SetActive(false);
        }
        protected override void DestroyObject(GameObject pooledObject)
        {
            GameObject.Destroy(pooledObject);
        }
    }
}
