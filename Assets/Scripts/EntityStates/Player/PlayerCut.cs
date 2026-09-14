using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CuttingEdge.EntityStates.Player
{
    public class PlayerCut : EntityState
    {
        public Vector2 startScreenPosition;
        public Vector2 endScreenPosition;
        public const float DURATION = 0.8f;
        public override void OnEnter()
        {
            base.OnEnter();
            // Rework cutting to not use planes, just boxcast with max distance?
            Vector3 startWorldPosition = Plane.enemyPlane.GetPositionFromScreenPosition(startScreenPosition);
            Vector3 endWorldPosition = Plane.enemyPlane.GetPositionFromScreenPosition(endScreenPosition);
            Debug.Log($"PlayerCut {startWorldPosition} -> {endWorldPosition}");
            Debug.DrawRay(startWorldPosition, endWorldPosition - startWorldPosition, Color.red, DURATION);
            RaycastHit[] hit = Physics.RaycastAll(startWorldPosition, endWorldPosition - startWorldPosition, Vector3.Distance(startWorldPosition, endWorldPosition), LayerCatalog.enemyHurtboxMask);
            bool cutHit = false;
            if (hit.Length > 0)
            {
                Vector3 cutNormal = Vector3.Cross((endWorldPosition - startWorldPosition).normalized, -Plane.enemyPlane.transform.forward);
                Debug.Log("Hit " + hit.Length);
                for (int i = 0; i < hit.Length; i++)
                {
                    if (hit[i].collider.TryGetComponent<ICuttable>(out var cuttable) && cuttable.TryCut(startWorldPosition, cutNormal))
                    {
                        cutHit = true;
                        Debug.Log("Cut");
                    }
                }
            }
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
