using DSGameUtils.Pools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CuttingEdge.Enemies
{
    public class ProjectileController : MonoBehaviour
    {
        [HideInInspector]
        public GameObject owner;
        public float speed;
        [SerializeField]
        private GameObject hitEffectPrefab;

        [HideInInspector]
        public Transform homingTarget;
        [Tooltip("Max degrees the projectile can turn in one second")]
        public float homingStrength;

        [SerializeField]
        private Collider projectileCollider;
        [SerializeField]
        private PooledGameObject pooledGameObject;

        public const float DEGREES_TO_RADIANS = 0.0174533f;

        void Update()
        {
            transform.position += transform.forward * speed * Time.deltaTime;

            if (homingTarget)
            {
                transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, homingTarget.position - transform.position, homingStrength * DEGREES_TO_RADIANS, Mathf.Infinity));
            }
        }
        [ContextMenu("Set Target To Player")]
        public void SetTargetToPlayer()
        {
            projectileCollider.gameObject.layer = LayerCatalog.playerHurtboxLayer;
        }
        [ContextMenu("Set Target To Enemy")]
        public void SetTargetToEnemy()
        {
            projectileCollider.gameObject.layer = LayerCatalog.enemyHurtboxLayer;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<IHurtBox>(out var hurtBox))
            {
                hurtBox.GetParent().Damage();
            }
            EffectManager.SimpleEffect(hitEffectPrefab, transform.position, transform.rotation);
            pooledGameObject.ReturnToPool();
        }
    }
}
