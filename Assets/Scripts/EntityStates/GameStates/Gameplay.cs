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
            if (EnemyManager.instance) 
            { 
                EnemyManager.instance.spawningEnemies = true;
            }
            if (InstanceTracker.Any<EnemyController>())
            {
                EnemyManager.onEnemiesDefeated += StartMusic;
            }
            else
            {
                StartMusic(0);
            }
            ScoreManager.Reset();
            onGameStarted?.Invoke();
        }
        private void StartMusic(int num)
        {
            if (!InstanceTracker.Any<EnemyController>())
            {
                MusicManager.instance.Play(Resources.Load<AudioClip>("Music/CuttingEdgeBattle"), 0f);
            }
        }
        public override void OnExit()
        {
            if (EnemyManager.instance) EnemyManager.instance.spawningEnemies = false;
            EnemyManager.onEnemiesDefeated -= StartMusic;
            TutorialManager.HideTutorialTextBox();
            MusicManager.instance.Stop(2f);
            base.OnExit();
        }
    }
}
