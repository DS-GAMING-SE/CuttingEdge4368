using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DSGameUtils.Pools
{
    public class GameObjectPool : Pool<GameObject>
    {
        public GameObject prefab;
        public GameObjectPool(GameObject prefab) : base()
        {
            this.prefab = prefab;
        }
        public GameObjectPool(GameObject prefab, int capacity) : base(capacity)
        {
            this.prefab = prefab;
        }
        public GameObjectPool(GameObject prefab, int capacity, int startCount) : base(capacity, startCount)
        {
            this.prefab = prefab;
        }
        public override GameObject Get()
        {
            GameObject pooledObject = base.Get();
            pooledObject.SetActive(true);
            return pooledObject;
        }
        protected override GameObject CreateNewPooledObject()
        {
            if (!prefab) { Debug.LogError("GameObjectPool prefab is null"); return null; }
            GameObject pooledObject = GameObject.Instantiate(prefab);
            pooledObject.hideFlags = HideFlags.DontSaveInEditor;
            foreach (Transform child in pooledObject.transform)
            {
                child.gameObject.hideFlags = HideFlags.DontSaveInEditor;
            }
            pooledObject.EnsureComponent<PooledGameObject>().pool = this;
            pooledObject.SetActive(false);
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
