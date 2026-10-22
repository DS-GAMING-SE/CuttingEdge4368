using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CuttingEdge
{
    [RequireComponent(typeof(Collider))]
    public class HurtBox : MonoBehaviour, IHurtBox
    {
        public HealthComponent healthComponent;
        public IDamageable GetDamageable()
        {
            return healthComponent;
        }
    }
    public interface IHurtBox
    {
        public IDamageable GetDamageable();
    }
}