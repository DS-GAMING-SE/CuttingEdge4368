using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController player;
    private void OnEnable()
    {
        if (player)
        {
            Destroy(gameObject);
            return;
        }
        player = this;
    }

    private void OnDisable()
    {
        if (player == this) player = null;
    }
}
