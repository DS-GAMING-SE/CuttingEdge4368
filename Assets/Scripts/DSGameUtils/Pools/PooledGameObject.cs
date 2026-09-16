using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace DSGameUtils.Pools
{
    public class PooledGameObject : MonoBehaviour
    {
        public GameObjectPool pool;
        [SerializeField]
        private Rigidbody rigidBody;
        public Action onReturnToPool;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ReturnToPool()
        {
            onReturnToPool?.Invoke();
            if (rigidBody)
            {
                rigidBody.velocity = Vector3.zero;
            }
            pool.Return(gameObject);
        }
        private void OnDestroy()
        {
            pool.Remove(gameObject);
        }
    }
}
