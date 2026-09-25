using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CuttingEdge
{
    public class HealthComponent : MonoBehaviour, IOnHitReceiver
    {
        public int maxHealth;
        public int health { get { return _health; } }
        private int _health;

        private void Start()
        {
            _health = maxHealth;
        }

        public void Damage()
        {
            _health -= 1;
        }
        public void SetHealth(int newHealth)
        {
            _health = newHealth;
        }
    }
    public interface IOnHitReceiver
    {
        public void Damage();
    }
}
