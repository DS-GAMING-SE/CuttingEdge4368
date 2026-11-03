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

    public static void Play(AudioClip musicTrack, float fadeTime)
    {
        if (!musicTrack || musicTrack == instance.activeMusicTrack) return;

        if (_instance.activeMusicTrack)
        {
            _instance.activeMusicPlayer.Stop(fadeTime);
        }

        _instance.SwitchActiveMusicPlayer();
        _instance.activeMusicTrack = musicTrack;
        _instance.activeMusicPlayer.Play(musicTrack, fadeTime);
    }
    public static void Stop(float fadeTime)
    {
        if (_instance && !_instance.activeMusicTrack) return;

        _instance.activeMusicTrack = null;
        if (_instance.activeMusicPlayer) _instance.activeMusicPlayer.Stop(fadeTime);
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
