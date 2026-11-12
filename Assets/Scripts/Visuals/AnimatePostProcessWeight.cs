using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class AnimatePostProcessWeight : MonoBehaviour
{
    [SerializeField]
    private PostProcessVolume ppVolume;
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
            if (ppVolume)
            {
                ppVolume.weight = propertyCurve.Evaluate(value);
            }
        }
    }
    private float _alpha;
}
