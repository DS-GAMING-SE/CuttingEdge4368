using CuttingEdge;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController player;
    private HealthComponent healthComponent;
    private void OnEnable()
    {
        if (player)
        {
            Destroy(gameObject);
            return;
        }
        player = this;
        healthComponent = GetComponent<HealthComponent>();
        if (healthComponent)
        {
            healthComponent.onDeath += EndGameOnDeath;
        }
    }

    private void OnDisable()
    {
        if (player == this) player = null;
        if (healthComponent)
        {
            healthComponent.onDeath -= EndGameOnDeath;
        }
    }

    private void EndGameOnDeath()
    {
        GameStateManager.instance.state = GameStateManager.GameState.Results;
    }
}
