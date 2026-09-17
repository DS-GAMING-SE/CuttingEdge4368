using DSGameUtils.Pools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimCutEffectController : MonoBehaviour
{
    [SerializeField]
    private LineRenderer lineRenderer;
    [SerializeField]
    private Transform endPoint;
    public PooledGameObject pooled;

    public void SetStartPosition(Vector3 position)
    {
        lineRenderer.SetPosition(0, transform.position);
    }
    public void SetEndPosition(Vector3 position)
    {
        endPoint.position = position;
        lineRenderer.SetPosition(1, position);
    }
}
