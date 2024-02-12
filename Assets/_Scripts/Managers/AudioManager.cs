using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///     Handles Music & SFX
/// </summary>
public class AudioManager : PersistentSingleton<AudioManager>
{
    [SerializeField] public AudioClip planktonMusic;



    public void PlaySFX(AudioSource audioSource, AudioClip audioClip)
    {

    }

    public void PlayRandomPitchSFX(AudioSource audioSource, AudioClip audioClip, float low = 0.75f, float high = 1.25f)
    {

    }

    public void StartMusic(AudioSource audioSource, AudioClip audioClip)
    {
         
    }
}