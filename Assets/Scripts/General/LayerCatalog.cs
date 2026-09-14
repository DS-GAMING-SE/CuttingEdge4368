using CuttingEdge.EntityStates;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace CuttingEdge
{
    public static class LayerCatalog
    {
        public static int enemyHurtboxLayer = LayerMask.NameToLayer("EnemyHurtbox");
        public static int enemyHurtboxMask = 1 << enemyHurtboxLayer;
    }
}
