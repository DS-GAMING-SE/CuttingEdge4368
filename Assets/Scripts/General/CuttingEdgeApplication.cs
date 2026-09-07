using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CuttingEdge
{
    public class CuttingEdgeApplication : MonoBehaviour
    {
        public static CuttingEdgeApplication instance;

        public static Action onFixedUpdate;
        public static Action onUpdate;
        public static Action onLateUpdate;
        public static Action onApplicationQuit;

        private void Awake()
        {
            GameObject.DontDestroyOnLoad(gameObject);
        }
        void OnEnable()
        {
            if (instance) Destroy(gameObject);

            instance = this;
        }

        private void OnDisable()
        {
            instance = null;
        }
        void FixedUpdate()
        {
            onFixedUpdate?.Invoke();
        }
        void Update()
        {
            onUpdate?.Invoke();
        }
        void LateUpdate()
        {
            onLateUpdate?.Invoke();
        }
        private void OnApplicationQuit()
        {
            onApplicationQuit?.Invoke();
        }
    }
}
