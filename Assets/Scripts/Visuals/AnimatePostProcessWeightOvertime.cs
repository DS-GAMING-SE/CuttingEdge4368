using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatePostProcessWeightOvertime : AnimatePostProcessWeight
{
    [SerializeField]
    protected float duration;
    private float timer;
    private void OnEnable()
    {
        timer = 0f;
        alpha = 0f;
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
