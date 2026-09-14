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
        public static string testText;
        private bool printed;
        [MenuItem("Test/TestText")]
        public static void SetTestText()
        {
            typeof(TestState).GetField("testText", BindingFlags.Static | BindingFlags.Public).SetValue(null, "Test Text");
        }
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
