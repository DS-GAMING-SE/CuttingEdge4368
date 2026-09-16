using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EzySlice;

public class EnemyHurtBox : MonoBehaviour, ICuttable
{
    [SerializeField]
    private Material cutMaterial;
    [SerializeField]
    private GameObject cutGameObject;
    public bool TryCut(Vector3 position, Vector3 normal)
    {
        if (!CanBeCut(position, normal) || !cutGameObject) return false;

        SlicedHull slice = cutGameObject.Slice(position, normal, cutMaterial);
        if (slice != null)
        {
            GameObject lowerHull = slice.CreateLowerHull(cutGameObject, cutMaterial);
            if (lowerHull && lowerHull.TryGetComponent<Rigidbody>(out var lowerRigidBody))
            {
                lowerRigidBody.AddForce(normal * -5, ForceMode.Impulse);
            }
            GameObject upperHull = slice.CreateUpperHull(cutGameObject, cutMaterial);
            if (upperHull && upperHull.TryGetComponent<Rigidbody>(out var upperRigidBody))
            {
                upperRigidBody.AddForce(normal * 5, ForceMode.Impulse);
            }
            GameObject.Destroy(cutGameObject); //replace with repooling
            return true;
        }
        return false;
    }
    protected virtual bool CanBeCut(Vector3 position, Vector3 normal)
    {
        return true;
    }
}
