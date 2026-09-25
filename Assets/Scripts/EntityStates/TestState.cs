using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace CuttingEdge.EntityStates
{
    public class TestState : EntityState
    {
        [ConfigSerializeField]
        public string testText{ get { return config.serializedStrings[0]; } }
        [ConfigSerializeField]
        public GameObject testGameObject{ get { return (GameObject)config.serializedObjects[0]; } }
        [ConfigSerializeField]
        public GameObject testGameObject2{ get { return (GameObject)config.serializedObjects[1]; } }
        [ConfigSerializeField]
        public GameObject testGameObject3{ get { return (GameObject)config.serializedObjects[2]; } }
        private bool printed;
        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (!string.IsNullOrEmpty(testText) && !printed)
            {
                Debug.Log(testText);
                printed = true;
            }
        }
    }
}
