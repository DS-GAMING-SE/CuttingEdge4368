using DSGameUtils.Pools;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace CuttingEdge
{
    public class ProjectileController : MonoBehaviour
    {
        [HideInInspector]
        public GameObject owner;
        public float speed { get { return overrideSpeed > 0 ? overrideSpeed : defaultSpeed; } set { overrideSpeed = value; } }
        [Header("Generic")]
        [SerializeField]
        private float defaultSpeed = 50f;
        private float overrideSpeed = -1f;
        [SerializeField]
        private GameObject hitEffectPrefab;
        [SerializeField]
        private int damage = 1;
        public bool parryable = true;

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
        public UnityEvent OnLeftTargetPlane;
        public UnityEvent OnReset;

        private Plane targetPlane;
        private Vector3 targetPointOnPlane;
        [NonSerialized]
        public bool onPlane;
        [NonSerialized]
        public bool destroyOnEndOfFrame; 

        public const float DEGREES_TO_RADIANS = 0.0174533f;

        private void OnEnable()
        {
            if (indicatorPrefab) indicator = DynamicGameObjectPool.CreateOrGetPooledObject(indicatorPrefab);
            overrideSpeed = -1f;
            destroyOnEndOfFrame = false;
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
        }
        [ContextMenu("Set Target To Enemy")]
        public void SetTargetToEnemy()
        {
            projectileCollider.gameObject.layer = LayerCatalog.enemyHurtboxLayer;
            targetPlane = Plane.enemyPlane;
        }
        public void ResetProjectile()
        {
            overrideSpeed = -1f;
            destroyOnEndOfFrame = false;
            OnReset?.Invoke();
            Debug.Log("Reset");
        }

        private void UpdateIndicatorPosition()
        {
            if (!targetPlane) return;
            if (targetPlane.TryRaycastOntoPlane(transform.position, transform.forward, out targetPointOnPlane))
            {
                onPlane = true;
            }
            else if (onPlane)
            {
                onPlane = false;
                OnLeftTargetPlane?.Invoke();
                Debug.Log("Left Plane");
            }

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
                destroyOnEndOfFrame = true;
                hurtBox.GetParent().Damage(damage, owner, gameObject);
            }
        }
        private void LateUpdate()
        {
            if (destroyOnEndOfFrame)
            {
                EffectManager.SimpleEffect(hitEffectPrefab, transform.position, transform.rotation);
                pooledGameObject.ReturnToPool();
            }
        }
    }
}
