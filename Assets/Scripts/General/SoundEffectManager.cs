using CuttingEdge;
using DSGameUtils.Pools;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SoundEffectManager
{
    public static GameObjectPool audioSourcePool;
    public static GameObjectPool dontDestroyAudioSourcePool;
    public static void PlaySound(AudioClip audioClip, bool dontDestroyOnLoad = false)
    {
        if (!audioClip) return;

        GameObject audioSourceObject;
        if (dontDestroyOnLoad)
        {
            audioSourceObject = GameObjectPool.GetOrCreatePooledGameObject(Resources.Load<GameObject>("SoundEffectObject"), ref dontDestroyAudioSourcePool);
        }
        else
        {
            audioSourceObject = GameObjectPool.GetOrCreatePooledGameObject(Resources.Load<GameObject>("SoundEffectObject"), ref audioSourcePool);
        }
        AudioSource audioSource = audioSourceObject.GetComponent<AudioSource>();
        audioSource.PlayOneShot(audioClip);
    }
    public static void PlaySound(params AudioClip[] randomAudioClips)
    {
        PlaySound(randomAudioClips[Random.Range(0, randomAudioClips.Length - 1)], false);
    }
    public static void PlaySound(AudioClip[] randomAudioClips, bool dontDestroyOnLoad = false)
    {
        PlaySound(randomAudioClips[Random.Range(0, randomAudioClips.Length - 1)], dontDestroyOnLoad);
    }
    
    [RuntimeInitializeOnLoadMethod]
    private static void Initialize()
    {
        SceneManager.activeSceneChanged += (scene, scene2) => { audioSourcePool?.Clear(); };
#if UNITY_EDITOR
        EditorApplication.playModeStateChanged += (state) => { dontDestroyAudioSourcePool?.Clear(); };
#endif
    }
}
