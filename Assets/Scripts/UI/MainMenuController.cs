using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [SerializeField]
    private GameObject mainMenuContainer;
    [SerializeField]
    private Button startButton;
    [SerializeField]
    private string startSceneName = "Gameplay";
    [SerializeField]
    private Button settingsButton;
    [SerializeField]
    private GameObject settingsMenuContainer;
    [SerializeField]
    private Button settingsBackButton;
    [SerializeField]
    private Button quitButton;

    private void Awake()
    {
        startButton.onClick.AddListener(StartButtonClicked);
        settingsButton.onClick.AddListener(SetSettingsMenuActive);
        settingsBackButton.onClick.AddListener(SetMainMenuActive);
        quitButton.onClick.AddListener(QuitButtonClicked);
    }

    private void StartButtonClicked()
    {
        SceneManager.LoadScene(startSceneName);
        MusicManager.instance.Stop(3f);
    }
    private void SetSettingsMenuActive()
    {
        settingsMenuContainer.SetActive(true);
        mainMenuContainer.SetActive(false);
    }
    private void SetMainMenuActive()
    {
        settingsMenuContainer.SetActive(false);
        mainMenuContainer.SetActive(true);
    }
    private void QuitButtonClicked()
    {
        Application.Quit();
    }
}
