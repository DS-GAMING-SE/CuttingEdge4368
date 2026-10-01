using System;
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

        [SerializeField]
        private float invincibilityDuration;
        private float invincibilityTimer;

        public bool invincible { get; private set; }

        [SerializeField]
        private GameObject hitEffectPrefab;
        [SerializeField]
        private AudioClip hitSoundEffect;
        [SerializeField]
        private AudioClip deathSoundEffect;
        [SerializeField]
        private GameObject healthBarPrefab;
        private HealthBar healthBar;

        // this will have problems if anything other than the player has healthcomponent, but there's no plans of that happening
        public static Action onDeath;

        private void Start()
        {
            _health = maxHealth;
        }
        private void OnEnable()
        {
            if (healthBarPrefab) 
            { 
                healthBar = GameObject.Instantiate(healthBarPrefab).GetComponent<HealthBar>();
                healthBar.healthComponent = this;
            }
        }

        private void OnDisable()
        {
            if (healthBar) Destroy(healthBar);
        }

        public void Damage()
        {
            if (invincible) return;
            
            SetHealth(health - 1);
            if (hitEffectPrefab) EffectManager.SimpleEffect(hitEffectPrefab, transform.position, Quaternion.identity);
            if (health <= 0)
            {
                if (deathSoundEffect) SoundEffectManager.PlaySound(deathSoundEffect);
                onDeath?.Invoke();
                Destroy(gameObject);
            }
            else
            {
                if (hitSoundEffect) SoundEffectManager.PlaySound(hitSoundEffect);
                invincible = true;
                invincibilityTimer = invincibilityDuration;
            }
        }
        public void SetHealth(int newHealth)
        {
            _health = newHealth;
            if (healthBar)
            {
                healthBar.SetFill(((float)health) / maxHealth);
            }
        }

        private void FixedUpdate()
        {
            if (invincibilityTimer > 0)
            {
                invincibilityTimer -= Time.fixedDeltaTime;
                if (invincibilityTimer <= 0)
                {
                    invincible = false;
                }
            }
        }
    }
    public interface IOnHitReceiver
    {
        public void Damage();
    }
}
