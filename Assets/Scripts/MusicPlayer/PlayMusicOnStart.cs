using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class PlayMusicOnStart : MonoBehaviour
{
    [SerializeField]
    private AudioClip music;
    [SerializeField]
    private float fadeTime = 1f;
    [SerializeField]
    private bool stopMusicOnDestroy;
    [SerializeField]
    private float stopFadeTime = 1f;

    void Start()
    {
        MusicManager.instance.Play(music, fadeTime);
    }

    public void StopMusic()
    {
        MusicManager.instance.Stop(stopFadeTime);
    }
}
