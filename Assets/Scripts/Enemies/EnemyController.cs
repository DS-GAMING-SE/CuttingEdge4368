using DSGameUtils.Pools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CuttingEdge.Enemies
{
    public class EnemyController : MonoBehaviour, IOnHitReceiver
    {
        public PooledGameObject pooledGameObject { get { return _pooledGameObject; } }
        private PooledGameObject _pooledGameObject;
        private void Awake()
        {
            _pooledGameObject = GetComponent<PooledGameObject>();
        }
        public void DefeatEnemy()
        {
            EnemyManager.onEnemyDefeated?.Invoke(this);
            if (pooledGameObject)
            {
                pooledGameObject.ReturnToPool();
            }
            else
            {
                GameObject.Destroy(gameObject);
            }
        }
        public void Damage()
        {

        }
    }
}
