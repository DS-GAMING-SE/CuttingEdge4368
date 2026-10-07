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
            return;
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
