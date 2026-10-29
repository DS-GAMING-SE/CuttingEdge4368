using CuttingEdge;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager instance
    {
        get { 
            if (!_instance)
            {
                _instance = FindAnyObjectByType<MusicManager>();
                if (!_instance)
                {
                    GameObject singleton = new GameObject("MusicManager");
                    singleton.AddComponent<MusicManager>();
                    DontDestroyOnLoad(singleton);
                }
            }
            return _instance;
        }
    }
    private static MusicManager _instance;

    private MusicPlayer musicPlayer1;
    private MusicPlayer musicPlayer2;
    private MusicPlayer activeMusicPlayer;

    public float volume
    {
        get => SettingsManager.musicVolume;
        set { SettingsManager.musicVolume = value;
            if (activeMusicPlayer) activeMusicPlayer.FadeVolume(value, 0f);
        }
    }

    private AudioClip activeMusicTrack;

    private void Awake()
    {
        if (instance && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        SetupMusicPlayers();
    }

    public void Play(AudioClip musicTrack, float fadeTime)
    {
        if (!musicTrack || musicTrack == activeMusicTrack) return;

        if (activeMusicTrack)
        {
            activeMusicPlayer.Stop(fadeTime);
        }

        SwitchActiveMusicPlayer();
        activeMusicTrack = musicTrack;
        activeMusicPlayer.Play(musicTrack, fadeTime);
    }
    public void Stop(float fadeTime)
    {
        if (!activeMusicTrack) return;

        activeMusicTrack = null;
        activeMusicPlayer.Stop(fadeTime);
    }
    private void SetupMusicPlayers()
    {
        musicPlayer1 = gameObject.AddComponent<MusicPlayer>();
        musicPlayer2 = gameObject.AddComponent<MusicPlayer>();
        activeMusicPlayer = musicPlayer1;
    }

    private void SwitchActiveMusicPlayer()
    {
        activeMusicPlayer = activeMusicPlayer == musicPlayer1 ? musicPlayer2 : musicPlayer1;
    }
}
