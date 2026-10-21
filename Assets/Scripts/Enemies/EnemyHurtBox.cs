using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EzySlice;

namespace CuttingEdge.Enemies
{
    public class EnemyHurtBox : MonoBehaviour, ICuttable, IHurtBox
    {
        [SerializeField]
        private Material crossSectionMaterial;
        [SerializeField]
        private GameObject cutGameObject;
        [SerializeField]
        private EnemyController enemyController;
        public bool TryCut(Vector3 position, Vector3 normal)
        {
            if (!CanBeCut() || !cutGameObject) return false;

            SlicedHull slice = cutGameObject.Slice(position, normal, crossSectionMaterial);
            if (slice != null)
            {
                GameObject lowerHull = slice.CreateLowerHull(cutGameObject, crossSectionMaterial);
                if (lowerHull)
                {
                    lowerHull.transform.SetParent(null);
                    if (lowerHull.TryGetComponent<Rigidbody>(out var lowerRigidBody))
                    {
                        lowerRigidBody.AddForce(normal * -10, ForceMode.Impulse);
                    }
                }
                GameObject upperHull = slice.CreateUpperHull(cutGameObject, crossSectionMaterial);
                if (upperHull)
                {
                    upperHull.transform.SetParent(null);
                    if (upperHull.TryGetComponent<Rigidbody>(out var upperRigidBody))
                    {
                        upperRigidBody.AddForce(normal * 10, ForceMode.Impulse);
                    }
                }
                if (enemyController)
                {
                    enemyController.DefeatEnemy();
                }
                else
                {
                    GameObject.Destroy(gameObject); //replace with repooling
                }
                return true;
            }
            return false;
        }
        public IDamageable GetParent()
        {
            return enemyController;
        }
        public virtual bool CanBeCut()
        {
            return true;
        }
    }
}
