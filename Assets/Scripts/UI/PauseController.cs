using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace CuttingEdge
{
    public class PauseController : MonoBehaviour
    {
        [SerializeField]
        private InputActionAsset inputActions;
        private InputAction pause;
        public bool paused;


        [SerializeField]
        private GameObject pauseMenu;
        [SerializeField]
        private Button resumeButton;
        [SerializeField]
        private Button mainMenuButton;
        [SerializeField]
        private string mainMenuSceneName = "MainMenu";
        [SerializeField]
        private Button quitButton;

        private TimeScaleManager.TimeScaleOverride timeScaleOverride;

        private void Awake()
        {
            if (resumeButton) resumeButton.onClick.AddListener(ResumeButtonClicked);
            if (mainMenuButton) mainMenuButton.onClick.AddListener(MainMenuButtonClicked);
            if (quitButton) quitButton.onClick.AddListener(QuitButtonClicked);
            inputActions.Enable();
            pause = inputActions.FindAction("Pause", true);
            pause.performed += TogglePause;
        }
        private void TogglePause(InputAction.CallbackContext context)
        {
            SetPaused(!paused);
        }
        public void SetPaused(bool paused)
        {
            if (this.paused == paused) return;

            pauseMenu.SetActive(paused);
            this.paused = paused;
            if (paused)
            {
                timeScaleOverride = TimeScaleManager.AddTimeScaleOverride(0f, 1f, 0f, TimeScaleManager.Priority.Pause);
            }
            else
            {
                TimeScaleManager.RemoveTimeScaleOverride(timeScaleOverride, 0.75f);
            }
        }

        private void ResumeButtonClicked()
        {
            SetPaused(false);
        }
        private void MainMenuButtonClicked()
        {
            SetPaused(false);
            SceneManager.LoadScene(mainMenuSceneName);
        }
        private void QuitButtonClicked()
        {
            Application.Quit();
        }
        private void OnDestroy()
        {
            inputActions.Disable();
        }
    }
}
