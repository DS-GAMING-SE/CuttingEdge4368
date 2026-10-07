using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class FadeImage : MonoBehaviour
{
    private Image image;
    [SerializeField]
    private float startAlpha = 1f;
    [SerializeField]
    private float targetAlpha = 0f;
    [SerializeField]
    private float duration = 0.5f;
    [SerializeField]
    private bool ignoreTimeScale = true;
    [SerializeField]
    private bool disableOnEnd;
    private void Awake()
    {
        image = GetComponent<Image>();
    }

    private void OnEnable()
    {
        image.CrossFadeAlpha(startAlpha, 0f, true);
        image.CrossFadeAlpha(targetAlpha, duration, ignoreTimeScale);
        if (disableOnEnd) StartCoroutine(DisableOnTimer(duration));
    }

    private IEnumerator DisableOnTimer(float time)
    {
        yield return ignoreTimeScale ? new WaitForSecondsRealtime(time) : new WaitForSeconds(time);
        gameObject.SetActive(false);
    }

}
