using CuttingEdge;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ResultsScreenController : MonoBehaviour
{
    [SerializeField]
    private InputActionAsset inputActions;
    [SerializeField]
    private GameObject resultsScreen;
    [SerializeField]
    private TextMeshProUGUI scoreText;
    [SerializeField]
    private Button restartButton;
    [SerializeField]
    private Button mainMenuButton;
    [SerializeField]
    private string mainMenuSceneName = "MainMenu";
    [SerializeField]
    private Button quitButton;

    private void Awake()
    {
        if (restartButton) restartButton.onClick.AddListener(RestartButtonClicked);
        if (mainMenuButton) mainMenuButton.onClick.AddListener(MainMenuButtonClicked);
        if (quitButton) quitButton.onClick.AddListener(QuitButtonClicked);
        GameStateManager.onGameStateChanged += ShowResultsScreen;
    }
    private void OnDisable()
    {
        GameStateManager.onGameStateChanged -= ShowResultsScreen;
    }
    private void ShowResultsScreen(GameStateManager.GameState state)
    {
        resultsScreen.SetActive(state == GameStateManager.GameState.Results);
        scoreText.text = "Score: " +ScoreHUDController.FormatScore(ScoreManager.score);
    }
    private void RestartButtonClicked()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    private void MainMenuButtonClicked()
    {
        SceneManager.LoadScene(mainMenuSceneName);
    }
    private void QuitButtonClicked()
    {
        Application.Quit();
    }
}
