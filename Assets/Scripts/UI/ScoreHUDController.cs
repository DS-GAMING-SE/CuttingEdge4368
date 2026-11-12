using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

namespace CuttingEdge
{
    public class ScoreHUDController : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI scoreText;
        [SerializeField]
        private TextMeshProUGUI highScoreText;
        [SerializeField]
        private Animator animator;
        
        private void OnEnable()
        {
            ScoreManager.onScoreChanged += UpdateScore;
            ScoreManager.onHighScoreChanged += UpdateHighScore;
            ScoreManager.onHighScoreBroken += HighScoreBroken;
            UpdateScore(ScoreManager.score);
            UpdateHighScore(ScoreManager.highScore);
        }
        private void OnDisable()
        {
            ScoreManager.onScoreChanged -= UpdateScore;
            ScoreManager.onHighScoreChanged -= UpdateHighScore;
            ScoreManager.onHighScoreBroken -= HighScoreBroken;
        }
        private void UpdateScore(int score)
        {
            if (scoreText) scoreText.text = FormatScore(score);
        }
        private void UpdateHighScore(int highScore)
        {
            if (highScoreText) highScoreText.text = FormatScore(highScore);
        }
        private void HighScoreBroken(int highScore)
        {
            if (animator)
            {
                animator.SetBool("HighScore", true);
            }
        }
        public static string FormatScore(int score)
        {
            StringBuilder stringBuilder = new StringBuilder();
            var scoreToString = score.ToString().ToCharArray();
            for (int i = 0; i < scoreToString.Length; i++)
            {
                if (i != 0 && (scoreToString.Length - i) % 3 == 0) stringBuilder.Append(" ");
                stringBuilder.Append(scoreToString[i]);
            }
            return stringBuilder.ToString();
        }
    }
}
