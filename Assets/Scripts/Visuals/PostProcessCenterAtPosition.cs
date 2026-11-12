using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PostProcessCenterAtPosition : MonoBehaviour
{
    [SerializeField]
    private PostProcessVolume ppVolume;
    private LensDistortion lensDistortion;
    private Vignette vignette;

    private void OnEnable()
    {
        if (ppVolume)
        {
            if (!lensDistortion)
            {
                lensDistortion = ppVolume.profile.GetSetting<LensDistortion>();
            }
            if (!vignette)
            {
                vignette = ppVolume.profile.GetSetting<Vignette>();
            }
        }
    }
    private void LateUpdate()
    {
        Vector3 screenPointPixels = Camera.main.WorldToScreenPoint(transform.position);
        if (lensDistortion)
        {
            lensDistortion.centerX.Override((screenPointPixels.x / Camera.main.pixelWidth) - 0.5f);
            lensDistortion.centerY.Override((screenPointPixels.y / Camera.main.pixelWidth) - 0.5f);
        }
        if (vignette)
        {
            vignette.center.Override(new Vector2(screenPointPixels.x / Camera.main.pixelWidth, screenPointPixels.y / Camera.main.pixelWidth));
        }
    }
    private void OnDestroy()
    {
        if (ppVolume) Destroy(ppVolume.profile);
    }
}
