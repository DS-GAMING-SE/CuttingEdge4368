using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimateShaderPropertyOvertime : AnimateShaderProperty
{
    [SerializeField]
    protected float duration;
    private float timer;
    void Update()
    {
        timer += Time.deltaTime;
        alpha = timer / duration;
        if (timer > duration && disableOnEnd)
        {
            enabled = false;
        }
    }
}
