using CuttingEdge.Enemies;
using DSGameUtils;
using DSGameUtils.Pools;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CuttingEdge
{
    public static class TutorialManager
    {
        private static GameObject tutorialCanvas;
        private static GameObject textBox;
        [RuntimeInitializeOnLoadMethod]
        private static void Initialize()
        {
            EnemyManager.onEnemiesDefeated += (num) => { if (InstanceTracker.Any<EnemyController>()) return; if (textBox) textBox.SetActive(false); };
            EntityStates.GameStates.Gameplay.onGameStarted += () =>
            { SpawnTutorialTextBox("Click and drag to aim a cut attack. Release to cut all enemies between the two points."); };
            EnemyManager.onWaveStarted += (wave) =>
            {
                switch (wave)
                {
                    case 5:
                        SpawnTutorialTextBox("Right click to parry incoming projectiles, sending them back towards where you're aiming.");
                        break;
                }
            };
            CutManager.onCut += (result) =>
            {
                if (result == CutManager.CutResults.Blocked) SpawnTutorialTextBox("Shielded enemies block all cutting attacks. Right click to parry a projectile into the shield to break it.");
            };
        }
        public static void SpawnTutorialTextBox(string text)
        {
            if (!tutorialCanvas) 
            {
                tutorialCanvas = GameObject.Instantiate(Resources.Load<GameObject>("TutorialCanvas"));
                textBox = tutorialCanvas.transform.GetChild(0).gameObject;
            }
            textBox.SetActive(true);
            textBox.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = text;
        }
    }
}

