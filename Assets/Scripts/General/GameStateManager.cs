using CuttingEdge.EntityStates;
using DSGameUtils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CuttingEdge
{
    public class GameStateManager : MonoBehaviour
    {
        public static GameStateManager instance;
        public static Action<GameState> onGameStateChanged;
        public GameState state
        {
            get { return _state; }
            set { _state = value; SetEntityState(value); }
        }
        [NonSerialized]
        private GameState _state;

        [SerializeField]
        private EntityStateMachine stateMachine;
        [SerializeField]
        [SerializableType.RequiredType(typeof(EntityState), "CuttingEdge.EntityStates.GameStates")]
        private SerializableType introEntityState;
        [SerializeField]
        [SerializableType.RequiredType(typeof(EntityState), "CuttingEdge.EntityStates.GameStates")]
        private SerializableType gameplayEntityState;
        [SerializeField]
        [SerializableType.RequiredType(typeof(EntityState), "CuttingEdge.EntityStates.GameStates")]
        private SerializableType resultsEntityState;
        private void OnEnable()
        {
            if (instance) { Destroy(gameObject); return; }
            instance = this;
        }
        private void OnDisable()
        {
            if (instance == this) instance = null;
        }
        private void SetEntityState(GameState state)
        {
            if (stateMachine)
            {
                SerializableType entityState;
                switch (state)
                {
                    case GameState.Intro:
                        entityState = introEntityState;
                        break;
                    case GameState.Gameplay:
                        entityState = gameplayEntityState;
                        break;
                    case GameState.Results:
                        entityState = resultsEntityState;
                        break;
                    default:
                        entityState = gameplayEntityState;
                        break;
                }
                stateMachine.SetNextState(entityState);
            }
            onGameStateChanged?.Invoke(state);
        }

        public enum GameState
        {
            Intro,
            Gameplay,
            Results
        }
    }
}
