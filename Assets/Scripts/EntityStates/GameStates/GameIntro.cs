using DSGameUtils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CuttingEdge.EntityStates.GameStates
{
    public class GameIntro : EntityState
    {
        public const float introDuration = 1f;

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (fixedAge >= introDuration)
            {
                GameStateManager.instance.state = GameStateManager.GameState.Gameplay;
            }
        }
    }
}
