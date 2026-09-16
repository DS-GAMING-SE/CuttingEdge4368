using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DSGameUtils;
using UnityEngine.InputSystem;

namespace CuttingEdge.EntityStates.Player
{
    public class PlayerMainState : EntityState
    {
        private EntityStateMachine weaponStateMachine;
        public const float MOVEMENT_SPEED = 9f;
        public const float ACCELERATION_TIME = 0.65f;
        private Vector3 velocity;
        public override void OnEnter()
        {
            base.OnEnter();
            Debug.Log("PlayerMainState Start");
            outer.inputBank.cut.performed += StartCut;
            if (TryGetComponent<ComponentLocator>(out var component))
            {
                weaponStateMachine = component.FindComponent<EntityStateMachine>("WeaponStateMachine");
            }
        }
        public override void Update()
        {
            base.Update();
            Vector3.SmoothDamp(velocity, MOVEMENT_SPEED * outer.inputBank.moveVector2.ToVector3XY(), ref velocity, ACCELERATION_TIME);
            transform.localPosition = Plane.playerPlane.ClampPositionToPaddedPlane(transform.localPosition + (velocity * Time.deltaTime));
        }
        public override void OnExit()
        {
            outer.inputBank.cut.performed -= StartCut;
            base.OnExit();
        }
        private void StartCut(InputAction.CallbackContext context)
        {
            weaponStateMachine.TryInterruptState(new PlayerAimCut(), InterruptPriority.Any);
        }
    }
}
