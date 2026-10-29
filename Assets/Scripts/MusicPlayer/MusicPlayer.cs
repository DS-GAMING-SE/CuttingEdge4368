using System.Collections;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    private AudioSource audioSource;
    private Coroutine lerpVolumeRoutine = null;
    private Coroutine stopRoutine = null;

    private void Awake()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.loop = true;
        audioSource.playOnAwake = false;
    }

    public void Play(AudioClip musicTrack, float fadeTime)
    {
        audioSource.volume = 0f;
        audioSource.clip = musicTrack;
        audioSource.Play();
        FadeVolume(MusicManager.instance.volume, fadeTime);
    }
    public void Stop(float fadeTime)
    {
        if (stopRoutine != null)
        {
            StopCoroutine(stopRoutine);
        }
        stopRoutine = StartCoroutine(StopRoutine(fadeTime));
    }
    public void FadeVolume(float targetVolume, float fadeTime)
    {
        targetVolume = Mathf.Clamp01(targetVolume);
        fadeTime = Mathf.Max(fadeTime, 0f);

        if (lerpVolumeRoutine != null)
        {
            StopCoroutine(lerpVolumeRoutine);
        }
        if (Mathf.Approximately(fadeTime, 0))
        {
            audioSource.volume = targetVolume;
            return;
        }
        lerpVolumeRoutine = StartCoroutine(LerpVolumeRoutine(targetVolume, fadeTime));
    }

    private IEnumerator LerpVolumeRoutine(float targetVolume, float fadeTime)
    {
        float startVolume = audioSource.volume;
        for (float elapsedTime = 0f; elapsedTime <= fadeTime; elapsedTime += Time.deltaTime)
        {
            audioSource.volume = Mathf.Lerp(startVolume, targetVolume, elapsedTime / fadeTime);
            yield return null;
        }
        audioSource.volume = targetVolume;
    }
    private IEnumerator StopRoutine(float fadeTime)
    {
        if (lerpVolumeRoutine != null)
        {
            StopCoroutine(lerpVolumeRoutine);
        }
        lerpVolumeRoutine = StartCoroutine(LerpVolumeRoutine(0, fadeTime));

        yield return lerpVolumeRoutine;
        audioSource.Stop();
    }
}
