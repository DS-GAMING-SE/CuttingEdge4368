using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DSGameUtils.Pools
{
    [RequireComponent(typeof(PooledGameObject))]
    public class ReturnToPoolOnTimer : MonoBehaviour
    {
        private PooledGameObject pooledObject;

        public float duration = 3f;

        private float timer;

        private void Awake()
        {
            pooledObject = GetComponent<PooledGameObject>();
        }

        private void FixedUpdate()
        {
            timer += Time.fixedDeltaTime;
            if (timer >= duration)
            {
                pooledObject.ReturnToPool();
            }
        }
    }
}
