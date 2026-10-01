using DSGameUtils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CuttingEdge.EntityStates.Player
{
    public class PlayerState : EntityState
    {
        protected ComponentLocator componentLocator;
        private int animatorComponentIndex = -1;
        public override void OnEnter()
        {
            base.OnEnter();
            componentLocator = GetComponent<ComponentLocator>();
            if (componentLocator)
            {
                animatorComponentIndex = componentLocator.FindComponentIndex("ModelAnimator");
            }
        }

        public void PlayAnimation(string stateName)
        {
            if (animatorComponentIndex > 0)
            {
                componentLocator.GetComponent<Animator>(animatorComponentIndex).Play(stateName);
            }
        }
    }
}
