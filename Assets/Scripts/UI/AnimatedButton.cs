using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimatedButton : Button
{
    [SerializeField]
    protected GameObject hoverGameObject;
    [SerializeField]
    protected AudioClip hoverSFX;
    [SerializeField]
    protected AudioClip pressedSFX;

    protected bool hovered;

    protected override void DoStateTransition(SelectionState state, bool instant)
    {
        base.DoStateTransition(state, instant);
        bool wasHovered = hovered;
        hovered = state == SelectionState.Highlighted || state == SelectionState.Pressed || state == SelectionState.Selected;
        if (hoverGameObject) hoverGameObject.SetActive(hovered);
        if (hoverSFX && hovered && !wasHovered)
        {
            SoundEffectManager.PlaySound(hoverSFX);
        }
        if (state == SelectionState.Pressed && pressedSFX)
        {
            SoundEffectManager.PlaySound(pressedSFX, true);
        }
    }
}
