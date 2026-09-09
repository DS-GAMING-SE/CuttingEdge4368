using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CuttingEdge.EntityStates.Player
{
    public class PlayerCut : EntityState
    {
        public Vector2 startScreenPosition;
        public Vector2 endPosition;
        public const float DURATION = 0.6f;
        public override void OnEnter()
        {
            base.OnEnter();
            // Rework cutting to not use planes, just boxcast with max distance?
            Debug.Log($"PlayerCut {Plane.enemyPlane.GetPositionFromScreenPosition(startScreenPosition)} -> {Plane.enemyPlane.GetPositionFromScreenPosition(endPosition)}");
            Debug.DrawLine(Plane.enemyPlane.GetPositionFromScreenPosition(startScreenPosition), Plane.enemyPlane.GetPositionFromScreenPosition(endPosition), Color.red, 1f);
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
