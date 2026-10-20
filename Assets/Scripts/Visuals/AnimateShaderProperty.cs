using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimateShaderProperty : MonoBehaviour
{
    [SerializeField]
    private string propertyName;
    private int propertyID = -1;

    [SerializeField]
    protected AnimationCurve propertyCurve;
    [SerializeField]
    protected bool disableOnEnd;
    public float alpha 
    {
        get { return _alpha; }
        set 
        { 
            _alpha = value; 
            if (materials != null)
            {
                foreach (var material in materials)
                {
                    targetRenderer.GetPropertyBlock(propertyBlock);
                    propertyBlock.SetFloat(propertyID, propertyCurve.Evaluate(value));
                    Debug.Log(value);
                    targetRenderer.SetPropertyBlock(propertyBlock);
                }
            }
        }
    }
    private float _alpha;
    [SerializeField]
    protected Renderer targetRenderer;
    protected Material[] materials;
    private MaterialPropertyBlock propertyBlock;

    private void Start()
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
        propertyBlock = new MaterialPropertyBlock();
    }
}
