using CuttingEdge.EntityStates;
using DSGameUtils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

namespace CuttingEdge
{
    public class EntityStateMachine : MonoBehaviour
    {
        [SerializableType.RequiredType(typeof(EntityState), "CuttingEdge.EntityStates")]
        public SerializableType initialState = typeof(EntityState);
        [SerializableType.RequiredType(typeof(EntityState), "CuttingEdge.EntityStates")]
        public SerializableType mainState = typeof(EntityState);
        public EntityState state { get; private set; }
        private EntityState nextState;
        private EntityStateConfiguration nextStateConfig;
        [NonSerialized]
        public InputBank inputBank;
        private void Awake()
        {
            inputBank = gameObject.GetComponent<InputBank>();
        }
        private void OnEnable()
        {
            SetState(initialState.CreateInstanceOfType<EntityState>());
        }
        private void FixedUpdate()
        {
            state.FixedUpdate();
        }
        private void Update()
        {
            state.Update();
        }
        private void LateUpdate()
        {
            state.LateUpdate();
            if (nextState != null)
            {
                SetState(nextState, nextStateConfig);
                nextState = null;
                nextStateConfig = null;
            }
        }
        private void OnDisable()
        {
            state?.OnExit();
        }

        public void SetNextState(EntityState state, EntityStateConfiguration config = null)
        {
            nextState = state;
            nextStateConfig = config;
        }
        public void SetNextState(SerializableType state)
        {
            nextState = state.CreateInstanceOfType<EntityState>();
            nextStateConfig = null;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetNextStateToMain()
        {
            SetNextState(mainState);
        }
        public bool TryInterruptState(EntityState newState, EntityStateConfiguration config, InterruptPriority interruptPriority)
        {
            if (state == null || state.GetInterruptPriority() <= interruptPriority)
            {
                SetNextState(newState, config);
                return true;
            }
            return false;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryInterruptState(EntityState newState, InterruptPriority interruptPriority)
        {
            return TryInterruptState(newState, null, interruptPriority);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryInterruptState(EntityStateConfiguration config, InterruptPriority interruptPriority)
        {
            return TryInterruptState(config.stateType.CreateInstanceOfType<EntityState>(), config, interruptPriority);
        }
        private void SetState(EntityState newState, EntityStateConfiguration config = null)
        {
            state?.OnExit();
            state = newState;
            if (state != null)
            {
                state.outer = this;
                state.config = config;
                state.OnEnter();
            }
        }
    }
}
