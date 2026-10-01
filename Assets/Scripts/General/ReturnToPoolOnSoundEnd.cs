using DSGameUtils.Pools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PooledGameObject), typeof(AudioSource))]
public class ReturnToPoolOnSoundEnd : MonoBehaviour
{
    [SerializeField]
    private AudioSource audioSource;
    [SerializeField]
    private PooledGameObject pooledGameObject;
    private bool played;
    
    private void FixedUpdate()
    {
        if (audioSource.isPlaying)
        {
            played = true;
        }
        else if (played)
        {
            pooledGameObject.ReturnToPool();
        }
    }
}
