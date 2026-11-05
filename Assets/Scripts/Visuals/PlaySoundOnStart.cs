using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySoundOnStart : MonoBehaviour
{
    [SerializeField]
    private AudioClip sound;
    // Start is called before the first frame update
    void OnEnable()
    {
        SoundEffectManager.PlaySound(sound);
    }
}
