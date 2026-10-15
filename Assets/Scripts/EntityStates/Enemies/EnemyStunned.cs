using DSGameUtils;
using DSGameUtils.Pools;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CuttingEdge.EntityStates.Enemy
{
    public class EnemyStunned : EntityState
    {
        public const float stunDuration = 5f;
        private GameObject stunEffect;

        public override void OnEnter()
        {
            base.OnEnter();
            stunEffect = EffectManager.SimpleEffect(Resources.Load<GameObject>("EnemyStunEffect"), transform.position, Quaternion.identity);
        }
        
        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (fixedAge >= stunDuration)
            {
                this.outer.SetNextStateToMain();
            }
        }

        public override void OnExit()
        {
            stunEffect.ReturnToPool();
            base.OnExit();
        }
    }
}
