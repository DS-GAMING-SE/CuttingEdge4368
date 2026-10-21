using CuttingEdge.Enemies;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CuttingEdge
{
    public static class ScoreManager
    {
        public static int score { get; private set; }
        public static Action<int> onScoreChanged;
        public static int highScore { get; private set; } = 3000;
        public static bool highScoreBrokenThisRun { get; private set; }
        public static Action<int> onHighScoreBroken;
        public static Action<int> onHighScoreChanged;

        public static void AddScore(int amount)
        {
            score += amount;
            onScoreChanged?.Invoke(score);
            if (score > highScore)
            {
                SetHighScore(score);
            }
        }
        public static void SetHighScore(int score)
        {
            highScore = score;
            if (!highScoreBrokenThisRun)
            {
                onHighScoreBroken?.Invoke(highScore);
                highScoreBrokenThisRun = true;
            }
            onHighScoreChanged?.Invoke(highScore);
        }
        public static void Reset()
        {
            score = 0;
            highScoreBrokenThisRun = false;
        }
        [RuntimeInitializeOnLoadMethod]
        private static void Initialize()
        {
            EnemyManager.onEnemiesDefeated += EnemiesDefeatedScore;
        }
        public const int SCORE_PER_ENEMY = 100;
        public const float SCORE_MULT_PER_ENEMY = 0.25f;
        private static void EnemiesDefeatedScore(int numEnemies)
        {
            AddScore((int)(numEnemies * SCORE_PER_ENEMY * (1 + (SCORE_MULT_PER_ENEMY * numEnemies))));
        }
    }
}
