using CuttingEdge;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI text;

    private double timer;
    private StringBuilder stringBuilder = new StringBuilder();
    private void Awake()
    {
        GameStateManager.onGameStateChanged += DisableOnGameEnd;
    }
    private void OnEnable()
    {
        timer = 0f;
    }
    private void OnDisable()
    {
        GameStateManager.onGameStateChanged -= DisableOnGameEnd;
    }
    private void DisableOnGameEnd(GameStateManager.GameState state)
    {
        enabled = state == GameStateManager.GameState.Gameplay;
    }

    private void Update()
    {
        timer += Time.deltaTime;
        text.text = GetFormattedTime();
    }

    private string GetFormattedTime()
    {
        stringBuilder.Clear();
        int minutes = (int)Math.Floor(timer / 60f);
        if (minutes < 10)
        {
            stringBuilder.Append(0);
        }
        stringBuilder.Append(minutes);
        stringBuilder.Append(':');
        int seconds = (int)Math.Floor(timer % 60f);
        if (seconds < 10)
        {
            stringBuilder.Append(0);
        }
        stringBuilder.Append(seconds); // show milliseconds on the timer?

        return stringBuilder.ToString();
    }
}
