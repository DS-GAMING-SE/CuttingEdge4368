using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DSGameUtils;

namespace CuttingEdge.EntityStates.Player
{
    public class PlayerMainState : EntityState
    {
        private EntityStateMachine weaponStateMachine;
        public override void OnEnter()
        {
            base.OnEnter();
            Debug.Log("PlayerMainState Start");
            if (TryGetComponent<ComponentLocator>(out var component))
            {
                weaponStateMachine = component.FindComponent<EntityStateMachine>("WeaponStateMachine");
            }
        }

        public override void Update()
        {
            base.Update();
            if (Input.GetMouseButtonDown(0))
            {
                weaponStateMachine.TryInterruptState(new PlayerAimCut(), InterruptPriority.Any);
            }
        }
    }
}
