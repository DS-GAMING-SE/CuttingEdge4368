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
        public static Action onGameStarted;
        public override void OnEnter()
        {
            base.OnEnter();
            if (EnemyManager.instance) EnemyManager.instance.spawningEnemies = true;
            ScoreManager.Reset();
            onGameStarted?.Invoke();
        }
        public override void OnExit()
        {
            if (EnemyManager.instance) EnemyManager.instance.spawningEnemies = false;
            base.OnExit();
        }
    }
}
