using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DSGameUtils.Pools
{
    [RequireComponent(typeof(PooledGameObject))]
    public class ReturnToPoolOnParticleEnd : MonoBehaviour
    {
        private PooledGameObject pooledObject;
        [SerializeField]
        private ParticleSystem particle;
        [SerializeField]
        private bool includeChildren;

        private void Awake()
        {
            pooledObject = GetComponent<PooledGameObject>();
            if (!particle) particle.GetComponentInChildren<ParticleSystem>();
        }

        private void FixedUpdate()
        {
            if (!particle || !particle.IsAlive(includeChildren))
            {
                pooledObject.ReturnToPool();
            }
        }
    }
}
