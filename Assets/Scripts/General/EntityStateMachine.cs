using CuttingEdge.EntityStates;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DSGameUtils;
using System.Runtime.CompilerServices;

namespace CuttingEdge
{
    public class EntityStateMachine : MonoBehaviour
    {
        public SerializableType initialState = typeof(EntityState);
        public SerializableType mainState = typeof(EntityState);
        public EntityState state { get; private set; }
        private EntityState nextState;

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
                SetState(nextState);
                nextState = null;
            }
        }
        private void OnDisable()
        {
            state?.OnExit();
        }

        public void SetNextState(EntityState state)
        {
            nextState = state;
        }
        public void SetNextState(SerializableType state)
        {
            nextState = state.CreateInstanceOfType<EntityState>();
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetNextStateToMain()
        {
            SetNextState(mainState);
        }
        private void SetState(EntityState newState)
        {
            state?.OnExit();
            state = newState;
            if (state != null)
            {
                state.outer = this;
                state.OnEnter();
            }
        }
    }
}
