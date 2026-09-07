using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace DSGameUtils.Pools
{
    public class PooledGameObject : MonoBehaviour
    {
        public GameObjectPool pool;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ReturnToPool()
        {
            pool.Return(gameObject);
        }
        private void OnDestroy()
        {
            pool.Remove(gameObject);
        }
    }
}
