using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimatedButton : Button
{
    [SerializeField]
    protected GameObject hoverGameObject;

    protected bool hovered;
    protected override void DoStateTransition(SelectionState state, bool instant)
    {
        base.DoStateTransition(state, instant);
        hovered = state == SelectionState.Highlighted || state == SelectionState.Pressed || state == SelectionState.Selected;
        if (hoverGameObject) hoverGameObject.SetActive(hovered);
    }
}
