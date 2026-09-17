using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CuttingEdge.EntityStates.Player
{
    public class PlayerParry : EntityState
    {
        public Vector2 startScreenPosition;
        public Vector2 endScreenPosition;
        public const float DURATION = 0.8f;
        public override void OnEnter()
        {
            base.OnEnter();
            EffectManager.SimpleEffect(Resources.Load<GameObject>("ParryEffect"), transform.position + new Vector3(1, 1, 1), Quaternion.identity);
        }
        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (fixedAge > DURATION)
            {
                outer.SetNextStateToMain();
            }
        }
        public override InterruptPriority GetInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
    }
}
