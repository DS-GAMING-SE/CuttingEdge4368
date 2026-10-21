using DSGameUtils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CuttingEdge.EntityStates.GameStates
{
    public class GameResults : EntityState
    {
        public override void OnExit()
        {
            base.OnExit();
            ScoreManager.Reset();
        }
    }
}
