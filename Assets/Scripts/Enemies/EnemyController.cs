using CuttingEdge.EntityStates.Enemy;
using DSGameUtils;
using DSGameUtils.Pools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CuttingEdge.Enemies
{
    public class EnemyController : MonoBehaviour, IDamageable
    {
        public PooledGameObject pooledGameObject { get { return _pooledGameObject; } }
        private PooledGameObject _pooledGameObject;
        [SerializeField]
        private EntityStateMachine stunOnHit;
        private void Awake()
        {
            _pooledGameObject = GetComponent<PooledGameObject>();
        }
        private void OnEnable()
        {
            InstanceTracker.Add(this);
        }
        private void OnDisable()
        {
            InstanceTracker.Remove(this);
        }
        public void DefeatEnemy()
        {
            InstanceTracker.Remove(this);
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
        public void Damage(int amount, GameObject attacker, GameObject inflictor)
        {
            if (stunOnHit)
            {
                stunOnHit.SetNextState(new EnemyStunned());
            }
        }
    }
}
