using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CuttingEdge.EntityStates.Player
{
    public class PlayerAimCut : EntityState
    {
        private Vector2 startScreenPosition;
        private const float MINIMUM_CUT_LENGTH = 50;
        private AimCutEffectController aimEffect;
        public override void OnEnter()
        {
            base.OnEnter();
            outer.inputBank.cut.canceled += TryCut;
            outer.inputBank.cancelCut.performed += CancelState;
            startScreenPosition = outer.inputBank.aimCutVector2;
            Vector3 startEffectPosition = Camera.main.ScreenToWorldPoint(new Vector3(startScreenPosition.x, startScreenPosition.y, Plane.enemyPlane.distanceFromCamera / 2));
            aimEffect = EffectManager.SimpleEffect(Resources.Load<GameObject>("AimCutEffect"), startEffectPosition, Quaternion.identity).GetComponent<AimCutEffectController>();
            aimEffect.SetStartPosition(startEffectPosition);
            aimEffect.SetEndPosition(startEffectPosition);
        }
        public override void Update()
        {
            base.Update();
            aimEffect.SetEndPosition(Camera.main.ScreenToWorldPoint(new Vector3(outer.inputBank.aimCutVector2.x, outer.inputBank.aimCutVector2.y, Plane.enemyPlane.distanceFromCamera / 2)));
            //Debug.DrawLine(Plane.enemyPlane.GetPositionFromScreenPosition(startScreenPosition), Plane.enemyPlane.GetPositionFromScreenPosition(outer.inputBank.aimCut.ReadValue<Vector2>()), Color.cyan, 0.1f);
        }
        public override void OnExit()
        {
            outer.inputBank.cut.canceled -= TryCut;
            outer.inputBank.cancelCut.performed -= CancelState;
            aimEffect.pooled.ReturnToPool();
            base.OnExit();
        }

        private void TryCut(InputAction.CallbackContext context)
        {
            if ((startScreenPosition - new Vector2(outer.inputBank.aimCutVector2.x, outer.inputBank.aimCutVector2.y)).magnitude < MINIMUM_CUT_LENGTH)
            {
                outer.SetNextStateToMain();
            }
            else
            {
                outer.SetNextState(new PlayerCut { startScreenPosition = startScreenPosition, endScreenPosition = outer.inputBank.aimCutVector2 });
            }
        }
        private void CancelState(InputAction.CallbackContext context)
        {
            outer.SetNextStateToMain();
        }
        public override InterruptPriority GetInterruptPriority()
        {
            return InterruptPriority.Skill;
        }
    }
}
