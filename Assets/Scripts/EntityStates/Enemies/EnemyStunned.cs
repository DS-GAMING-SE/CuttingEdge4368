using DSGameUtils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CuttingEdge.EntityStates.Enemy
{
    public class EnemyStunned : EntityState
    {
        public const float stunDuration = 5f;

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (fixedAge >= stunDuration)
            {
                this.outer.SetNextStateToMain();
            }
        }
    }
}
