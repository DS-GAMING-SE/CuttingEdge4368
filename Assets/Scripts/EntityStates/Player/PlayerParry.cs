using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

namespace CuttingEdge.EntityStates.Player
{
    public class PlayerParry : EntityState
    {
        public Vector2 startScreenPosition;
        public Vector2 endScreenPosition;
        public const float DURATION = 0.65f;
        public const float PARRY_DURATION = 0.45f;
        public const float PARRY_INVINCIBILITY_DURATION = 0.6f;
        public const float PROJECTILE_SPEED_MULT = 3f;
        private bool parrying;
        private HealthComponent healthComponent;
        public override void OnEnter()
        {
            base.OnEnter();
            healthComponent = GetComponent<HealthComponent>();
            healthComponent.onHit += Parry;
            parrying = true;
            EffectManager.SimpleEffect(Resources.Load<GameObject>("ParryEffect"), transform.position + new Vector3(1, 1, 1), Quaternion.identity);
        }
        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (fixedAge > PARRY_DURATION && parrying)
            {
                parrying = false;
                healthComponent.onHit -= Parry;
            }
            if (fixedAge > DURATION)
            {
                outer.SetNextStateToMain();
            }
        }
        public override InterruptPriority GetInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
        public void Parry(ref int amount, ref bool rejected, GameObject attacker, GameObject inflictor)
        {
            if (inflictor && inflictor.TryGetComponent<ProjectileController>(out var projectile) && projectile.parryable)
            {
                rejected = true;
                healthComponent.SetInvincible(PARRY_INVINCIBILITY_DURATION);

                projectile.ResetProjectile();
                projectile.SetTargetToEnemy();
                Vector3 positionOnEnemyPlane = Plane.enemyPlane.GetPositionFromScreenPosition(outer.inputBank.aimCutVector2);
                /*if (Physics.Linecast(Camera.main.ScreenToWorldPoint(outer.inputBank.aimCutVector2), positionOnEnemyPlane, out var targetInfo, LayerCatalog.enemyHurtboxMask))
                {
                    if (targetInfo.colliderInstanceID != 0)
                    {
                        projectile.target = targetInfo.collider.transform;
                    }
                }
                else
                {
                    projectile.target = null;
                }*/
                projectile.target = null;
                projectile.transform.rotation = Quaternion.LookRotation(positionOnEnemyPlane - transform.position);
                projectile.speed *= 2f;

                parrying = false;
                healthComponent.onHit -= Parry;
            }
        }
    }
}
