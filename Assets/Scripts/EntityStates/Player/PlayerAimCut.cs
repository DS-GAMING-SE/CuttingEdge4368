using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CuttingEdge.EntityStates.Player
{
    public class PlayerAimCut : EntityState
    {
        private Vector2 startScreenPosition;
        public override void OnEnter()
        {
            startScreenPosition = Input.mousePosition;
        }
        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (Input.GetMouseButtonUp(0))
            {
                outer.SetNextState(new PlayerCut { startPosition = startScreenPosition, endPosition = Input.mousePosition});
            }
        }
    }
}
