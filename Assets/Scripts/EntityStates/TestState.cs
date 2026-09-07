using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CuttingEdge.EntityStates
{
    public class TestState : EntityState
    {
        public override void OnEnter()
        {
            Debug.Log("TYPE SERIALIZATION WORKS!!!!");
        }
    }
}
