using DSGameUtils.Pools;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.Rendering.DebugUI;

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
        [Tooltip("Max degrees the projectile can turn in one second")]
        [SerializeField]
        private float homingRadius;
        private float homingRadiusSqr;
        private Vector3 homingCenter;

        [Header("Indicator")]
        [SerializeField]
        private GameObject indicatorPrefab;
        private GameObject indicator;
        public float indicatorScale { get { return _indicatorScale; } set { _indicatorScale = value; if (indicator) indicator.transform.localScale = new Vector3(value, value, value); } }
        [SerializeField]
        private float _indicatorScale = 1;
        [SerializeField]
        private GameObject homingIndicatorPrefab;
        private GameObject homingIndicator;
        private List<AnimateShaderProperty> indicatorAnimates = new List<AnimateShaderProperty>();

        [HideInInspector]
        public Transform target
        {
            get { return _target; }
            set 
            { 
                _target = value;
                if (value) 
                { 
                    homingCenter = value.position;
                    if (homingIndicator)
                    {
                        homingIndicator.transform.position = homingCenter;
                    }
                }
                else if (homingIndicator)
                {
                    homingIndicator.ReturnToPool();
                    homingIndicator = null;
                }
            }
        }
        private Transform _target;

        [Header("Components")]
        [SerializeField]
        private Collider projectileCollider;
        [SerializeField]
        private PooledGameObject pooledGameObject;
        public UnityEvent OnLeftTargetPlane;
        public UnityEvent OnReset;
        private float suppressPlaneCheckTimer = 0f;

        private Plane targetPlane;
        private Vector3 targetPointOnPlane;
        [NonSerialized]
        public bool onPlane;
        [NonSerialized]
        public bool destroyOnEndOfFrame;
        private float startDistanceToTarget;
        private float distanceToTarget;

        private void Awake()
        {
            homingRadiusSqr = homingRadius * homingRadius;
        }

        private void OnEnable()
        {
            if (indicatorPrefab) 
            { 
                indicator = DynamicGameObjectPool.CreateOrGetPooledObject(indicatorPrefab); 
                if (indicator.TryGetComponent<AnimateShaderProperty>(out var indicatorAnimate))
                {
                    indicatorAnimates.Add(indicatorAnimate);
                }
                indicator.transform.localScale = new Vector3(indicatorScale, indicatorScale, indicatorScale);
            }
            if (homingRadius > 0 && homingIndicatorPrefab) 
            { 
                homingIndicator = DynamicGameObjectPool.CreateOrGetPooledObject(homingIndicatorPrefab);
                homingIndicator.transform.position = homingCenter;
                homingIndicator.transform.localScale = new Vector3(homingRadius, homingRadius, homingRadius);
                if (homingIndicator.TryGetComponent<AnimateShaderProperty>(out var homingIndicatorAnimate))
                {
                    indicatorAnimates.Add(homingIndicatorAnimate);
                }
            }
            ResetProjectile();
            UpdateOnPlane();
            UpdateIndicators();
        }
        private void OnDisable()
        {
            if (indicator)
            {
                indicator.ReturnToPool();
            }
            if (homingIndicator)
            {
                homingIndicator.ReturnToPool();
                homingIndicator = null;
            }
            indicatorAnimates.Clear();
        }
        void Update()
        {
            transform.position += speed * Time.deltaTime * transform.forward;
            if (suppressPlaneCheckTimer > 0)
            {
                suppressPlaneCheckTimer -= Time.deltaTime;
            }

            if (target)
            {
                if (homingRadius > 0 && onPlane && (target.position - homingCenter).sqrMagnitude < homingRadiusSqr)
                {
                    transform.rotation = Quaternion.LookRotation(target.position - transform.position);//Vector3.RotateTowards(transform.forward, target.position - transform.position, 90f * Time.deltaTime * Mathf.Deg2Rad, Mathf.Infinity));
                }
            }
            UpdateOnPlane();
            UpdateIndicators();
        }
        [ContextMenu("Set Target To Player")]
        public void SetTargetToPlayer()
        {
            projectileCollider.gameObject.layer = LayerCatalog.playerHurtboxLayer;
            targetPlane = Plane.playerPlane;
            suppressPlaneCheckTimer = 0.1f;
        }
        [ContextMenu("Set Target To Enemy")]
        public void SetTargetToEnemy()
        {
            projectileCollider.gameObject.layer = LayerCatalog.enemyHurtboxLayer;
            targetPlane = Plane.enemyPlane;
            suppressPlaneCheckTimer = 0.1f;
        }
        public void ResetProjectile()
        {
            overrideSpeed = -1f;
            destroyOnEndOfFrame = false;
            suppressPlaneCheckTimer = 0f;
            startDistanceToTarget = -1f;
            OnReset?.Invoke();
        }
        private void UpdateOnPlane()
        {
            if (!targetPlane || suppressPlaneCheckTimer > 0) return;
            if (targetPlane.TryRaycastOntoPlane(transform.position, transform.forward, out targetPointOnPlane, out distanceToTarget))
            {
                if (startDistanceToTarget == -1)
                {
                    startDistanceToTarget = distanceToTarget;
                }
                onPlane = true;
            }
            else if (onPlane)
            {
                onPlane = false;
                OnLeftTargetPlane?.Invoke();
            }
        }

        private void UpdateIndicators()
        {
            if (!indicator) return;
            if (onPlane)
            {
                foreach (var indicator in indicatorAnimates)
                {
                    indicator.alpha = 1 - (distanceToTarget / startDistanceToTarget);
                }
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
                hurtBox.GetDamageable().Damage(damage, owner, gameObject);
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
