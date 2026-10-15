using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CuttingEdge.Enemies
{
    [CreateAssetMenu(fileName = "spawn", menuName = "SpawnCard")]
    public class SpawnCard : ScriptableObject
    {
        public GameObject prefab;
        public int spawnCost;
    }
}
