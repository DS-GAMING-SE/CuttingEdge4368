using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [SerializeField]
    private Button startButton;
    [SerializeField]
    private string startSceneName = "Gameplay";
    [SerializeField]
    private Button settingsButton;
    [SerializeField]
    private Button quitButton;

    private void Awake()
    {
        startButton.onClick.AddListener(StartButtonClicked);
        settingsButton.onClick.AddListener(SettingsButtonClicked);
        quitButton.onClick.AddListener(QuitButtonClicked);
    }

    private void StartButtonClicked()
    {
        SceneManager.LoadScene(startSceneName);
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(startSceneName));
    }
    private void SettingsButtonClicked()
    {

    }
    private void QuitButtonClicked()
    {
        Application.Quit();
    }
}
