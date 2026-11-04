using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IProjectileIndicator
{
    public void SetClampedDistance(float distance01);
    public void SetTargeting(bool targeting);
}
