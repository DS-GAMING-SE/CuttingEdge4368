using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericProjectileIndicator : MonoBehaviour, IProjectileIndicator
{
    [SerializeField]
    private AnimateShaderProperty distanceEffect;
    [SerializeField]
    private GameObject targetingEffect;

    public void SetClampedDistance(float distance01)
    {
        if (distanceEffect) distanceEffect.alpha = distance01;
    }

    public void SetTargeting(bool targeting)
    {
        if (targetingEffect) targetingEffect.SetActive(targeting);
    }
}
