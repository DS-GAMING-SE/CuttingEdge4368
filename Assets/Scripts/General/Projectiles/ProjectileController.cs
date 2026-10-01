using DSGameUtils.Pools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CuttingEdge
{
    public class ProjectileController : MonoBehaviour
    {
        [HideInInspector]
        public GameObject owner;
        [Header("Generic")]
        public float speed = 30;
        [SerializeField]
        private GameObject hitEffectPrefab;

        [Header("Indicator")]
        [SerializeField]
        private GameObject indicatorPrefab;
        private GameObject indicator;
        public float indicatorScale { get { return _indicatorScale; } set { _indicatorScale = value; if (indicator) indicator.transform.localScale = new Vector3(value, value, value); } }
        [SerializeField]
        private float _indicatorScale = 1;

        [HideInInspector]
        public Transform target;
        [Header("Homing")]
        [Tooltip("Max degrees the projectile can turn in one second")]
        public float homingStrength;

        [Header("Components")]
        [SerializeField]
        private Collider projectileCollider;
        [SerializeField]
        private PooledGameObject pooledGameObject;

        private bool targetingPlayer;
        private Plane targetPlane;
        private Vector3 targetPointOnPlane;
        public bool onPlane;

        public const float DEGREES_TO_RADIANS = 0.0174533f;

        private void OnEnable()
        {
            if (indicatorPrefab) indicator = DynamicGameObjectPool.CreateOrGetPooledObject(indicatorPrefab);
            UpdateIndicatorPosition();
        }
        private void OnDisable()
        {
            if (indicator)
            {
                indicator.ReturnToPool();
            }
        }
        void Update()
        {
            transform.position += transform.forward * speed * Time.deltaTime;

            if (target)
            {
                if (homingStrength > 0 && onPlane)
                {
                    transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, target.position - transform.position, homingStrength * DEGREES_TO_RADIANS, Mathf.Infinity));
                }
            }
            UpdateIndicatorPosition();
        }
        [ContextMenu("Set Target To Player")]
        public void SetTargetToPlayer()
        {
            projectileCollider.gameObject.layer = LayerCatalog.playerHurtboxLayer;
            targetPlane = Plane.playerPlane;
            targetingPlayer = true;
        }
        [ContextMenu("Set Target To Enemy")]
        public void SetTargetToEnemy()
        {
            projectileCollider.gameObject.layer = LayerCatalog.enemyHurtboxLayer;
            targetPlane = Plane.enemyPlane;
            targetingPlayer = false;
        }

        private void UpdateIndicatorPosition()
        {
            if (!targetPlane) return;
            onPlane = targetPlane.TryRaycastOntoPlane(transform.position, transform.forward, out targetPointOnPlane);

            if (!indicator) return;
            if (onPlane)
            {
                indicator.SetActive(true);
                indicator.transform.position = targetPointOnPlane;
            }
            else
            {
                indicator.SetActive(false);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!isActiveAndEnabled) return;
            if (other.TryGetComponent<IHurtBox>(out var hurtBox))
            {
                hurtBox.GetParent().Damage();
            }
            EffectManager.SimpleEffect(hitEffectPrefab, transform.position, transform.rotation);
            pooledGameObject.ReturnToPool();
        }
    }
}
