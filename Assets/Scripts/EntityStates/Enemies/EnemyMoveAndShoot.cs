using DSGameUtils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CuttingEdge.EntityStates.Enemy
{
    public class EnemyMoveAndShoot : EntityState
    {
        [ConfigSerializeField]
        public GameObject projectilePrefab { get { return (GameObject)config.serializedObjects[0]; } }
        [ConfigSerializeField]
        public AudioClip projectileFireAudioClip { get { return (AudioClip)config.serializedObjects[1]; } }
        [ConfigSerializeField]
        public float timeBetweenShots { get { return config.serializedFloats[1]; } }
        [ConfigSerializeField]
        public float movementSpeed { get { return config.serializedFloats[0]; } }
        private float shootTimer;
        private ChildLocator childLocator;
        private Transform muzzleTransform;
        private Vector3 targetMovePosition;
        // Add something to EnemyController with a series of points defining a flight path. Replace targetMovePosition here with reading from EnemyController

        public override void OnEnter()
        {
            base.OnEnter();
            childLocator = GetComponent<ChildLocator>();
            if (childLocator)
            {
                muzzleTransform = childLocator.FindChild("Muzzle");
            }
            if (!muzzleTransform)
            {
                muzzleTransform = transform;
            }
            shootTimer = timeBetweenShots / 2f;
            targetMovePosition = Plane.enemyPlane.GetRandomWorldPositionOnPlane();
        }
        public override void FixedUpdate()
        {
            base.FixedUpdate();
            shootTimer += Time.fixedDeltaTime;
            if (shootTimer >= timeBetweenShots)
            {
                shootTimer -= timeBetweenShots;
                if (PlayerController.player)
                {
                    ProjectileManager.FireProjectile(projectilePrefab, muzzleTransform.position, gameObject, PlayerController.player.transform);
                    SoundEffectManager.PlaySound(projectileFireAudioClip);
                }
            }
        }
        public override void Update()
        {
            base.Update();
            transform.position = Vector3.MoveTowards(transform.position, targetMovePosition, movementSpeed * Time.deltaTime);
        }
    }
}
