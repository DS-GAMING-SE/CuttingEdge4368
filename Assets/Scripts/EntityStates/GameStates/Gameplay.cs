using CuttingEdge.Enemies;
using DSGameUtils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CuttingEdge.EntityStates.GameStates
{
    public class Gameplay : EntityState
    {
        public override void OnEnter()
        {
            base.OnEnter();
            if (EnemyManager.instance) EnemyManager.instance.spawningEnemies = true;
        }
        public override void OnExit()
        {
            if (EnemyManager.instance) EnemyManager.instance.spawningEnemies = false;
            base.OnExit();
        }
    }
}
