using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimateShaderProperty : MonoBehaviour
{
    [SerializeField]
    private string propertyName;
    private int propertyID = -1;

    [SerializeField]
    private AnimationCurve propertyCurve;
    [SerializeField]
    private float duration;
    [SerializeField]
    private bool disableOnEnd;
    private float timer;
    [SerializeField]
    private Renderer targetRenderer;
    private Material[] materials;
    
    void OnEnable()
    {
        if (propertyID == -1) propertyID = Shader.PropertyToID(propertyName);
        if (propertyID == -1)
        {
            Debug.LogError("Could not find property id for AnimateShaderProperty with property " + propertyName);
            enabled = false;
            return;
        }
        if (!targetRenderer)
        {
            Debug.LogError("No TargetRenderer is assigned to AnimateShaderProperty");
            enabled = false;
            return;
        }
        materials = targetRenderer.materials;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        foreach (var material in materials)
        {
            material.SetFloat(propertyID, propertyCurve.Evaluate(timer / duration));
        }
        if (timer > duration && disableOnEnd)
        {
            enabled = false;
        }
    }
}
