using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CuttingEdge.EntityStates.Player
{
    public class PlayerAimCut : EntityState
    {
        private Vector2 startScreenPosition;
        private const float MINIMUM_CUT_LENGTH = 50;
        public override void OnEnter()
        {
            base.OnEnter();
            startScreenPosition = Input.mousePosition;
        }
        public override void Update()
        {
            base.Update();
            Debug.DrawLine(Plane.enemyPlane.GetPositionFromScreenPosition(startScreenPosition), Plane.enemyPlane.GetPositionFromScreenPosition(Input.mousePosition), Color.cyan, 0.1f);
            if (Input.GetMouseButtonUp(0))
            {
                if ((startScreenPosition - new Vector2(Input.mousePosition.x, Input.mousePosition.y)).magnitude < MINIMUM_CUT_LENGTH)
                {
                    outer.SetNextStateToMain();
                }
                else
                {
                    outer.SetNextState(new PlayerCut { startScreenPosition = startScreenPosition, endPosition = Input.mousePosition });
                }
            }
        }
        public override InterruptPriority GetInterruptPriority()
        {
            return InterruptPriority.Skill;
        }
    }
}
