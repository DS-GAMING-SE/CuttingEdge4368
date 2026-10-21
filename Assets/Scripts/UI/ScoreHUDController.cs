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
        private GameObject highScoreBrokenEffect;
        
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
            if (highScoreBrokenEffect) highScoreBrokenEffect.SetActive(true);
            if (highScoreText) highScoreText.gameObject.SetActive(false);
        }
        public static string FormatScore(int score)
        {
            StringBuilder stringBuilder = new StringBuilder();
            var scoreToString = score.ToString().ToCharArray();
            for (int i = 0; i < scoreToString.Length; i++)
            {
                if ((scoreToString.Length - i) % 3 == 0) stringBuilder.Append(" ");
                stringBuilder.Append(scoreToString[i]);
            }
            return stringBuilder.ToString();
        }
    }
}
