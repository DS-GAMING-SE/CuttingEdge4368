using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CuttingEdge
{
    public class HealthComponent : MonoBehaviour, IDamageable
    {
        public int maxHealth;
        public int health { get { return _health; } }
        private int _health;

        [SerializeField]
        private float onHitInvincibilityDuration;
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
        [SerializeField]
        private bool onlyShowHealthBarDuringGameplay = true;

        public delegate void OnHitDelegate(ref int damage, ref bool rejected, GameObject attacker, GameObject inflictor);
        public event OnHitDelegate onHit;
        public Action onDeath;

        private void Start()
        {
            _health = maxHealth;
        }
        private void OnEnable()
        {
            if (healthBarPrefab) 
            { 
                if (!onlyShowHealthBarDuringGameplay || (GameStateManager.instance && GameStateManager.instance.state == GameStateManager.GameState.Gameplay))
                {
                    CreateHealthBar();
                }
                else
                {
                    GameStateManager.onGameStateChanged += OnGameStateChanged;
                }
            }
        }
        private void OnGameStateChanged(GameStateManager.GameState state)
        {
            if (state == GameStateManager.GameState.Gameplay)
            {
                GameStateManager.onGameStateChanged -= OnGameStateChanged;
                CreateHealthBar();
            }
        }
        private void CreateHealthBar()
        {
            if (healthBar) return;
            healthBar = GameObject.Instantiate(healthBarPrefab).GetComponent<HealthBar>();
            healthBar.healthComponent = this;
        }

        private void OnDisable()
        {
            if (healthBar) Destroy(healthBar);
            GameStateManager.onGameStateChanged -= OnGameStateChanged;
        }

        public void Damage(int amount, GameObject attacker, GameObject inflictor)
        {
            bool damageRejected = invincible;
            onHit?.Invoke(ref amount, ref damageRejected, attacker, inflictor);
            
            if (damageRejected) return;
            
            SetHealth(health - amount);
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
                SetInvincible(onHitInvincibilityDuration);
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
        public void SetInvincible(float duration)
        {
            invincibilityTimer = Mathf.Max(invincibilityTimer, duration);
            invincible = true;
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
    public interface IDamageable
    {
        public void Damage(int amount, GameObject attacker, GameObject inflictor);
    }
}
