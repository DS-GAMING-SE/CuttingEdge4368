using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimateShaderPropertyOvertime : AnimateShaderProperty
{
    [SerializeField]
    protected float duration;
    private float timer;
    private void OnEnable()
    {
        timer = 0f;
    }
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
