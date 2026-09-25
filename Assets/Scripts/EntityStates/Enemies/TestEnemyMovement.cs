using DSGameUtils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CuttingEdge.EntityStates.Enemy
{
    public class TestEnemyMovement : EntityState
    {
        private Vector3 targetPosition;
        //[ConfigSerializeField]
        public float speed = 7f;
        private float endTime;
        private bool endStarted;

        public override void OnEnter()
        {
            base.OnEnter();
            endTime = UnityEngine.Random.Range(1f, 4f);
            SetRandomTargetPosition();
        }
        public override void Update()
        {
            base.Update();
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
            if (age < endTime)
            {
                if (Vector3.kEpsilonNormalSqrt > (transform.position - targetPosition).sqrMagnitude)
                {
                    SetRandomTargetPosition();
                }
            }
            else
            {
                if (!endStarted)
                {
                    targetPosition = transform.position;
                    targetPosition.z = 0f;
                    speed *= 1.3f;
                    endStarted = true;
                }
                if (Vector3.kEpsilonNormalSqrt > (transform.position - targetPosition).sqrMagnitude)
                {
                    GameObject.Destroy(gameObject);
                }
            }
        }
        private void SetRandomTargetPosition()
        {
            targetPosition = Plane.enemyPlane.GetRandomWorldPositionOnPlane();
        }
    }
}
