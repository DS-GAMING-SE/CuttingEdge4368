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
        public override void OnEnter()
        {
            base.OnEnter();
            outer.inputBank.cut.canceled += TryCut;
            outer.inputBank.cancelCut.performed += CancelState;
            startScreenPosition = outer.inputBank.aimCut.ReadValue<Vector2>();
        }
        public override void Update()
        {
            base.Update();
            Debug.DrawLine(Plane.enemyPlane.GetPositionFromScreenPosition(startScreenPosition), Plane.enemyPlane.GetPositionFromScreenPosition(outer.inputBank.aimCut.ReadValue<Vector2>()), Color.cyan, 0.1f);
        }
        public override void OnExit()
        {
            outer.inputBank.cut.canceled -= TryCut;
            outer.inputBank.cancelCut.performed -= CancelState;
            base.OnExit();
        }

        private void TryCut(InputAction.CallbackContext context)
        {
            if ((startScreenPosition - new Vector2(outer.inputBank.aimCut.ReadValue<Vector2>().x, outer.inputBank.aimCut.ReadValue<Vector2>().y)).magnitude < MINIMUM_CUT_LENGTH)
            {
                outer.SetNextStateToMain();
            }
            else
            {
                outer.SetNextState(new PlayerCut { startScreenPosition = startScreenPosition, endScreenPosition = outer.inputBank.aimCut.ReadValue<Vector2>() });
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
