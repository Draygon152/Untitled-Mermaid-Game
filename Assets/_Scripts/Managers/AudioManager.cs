using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

/// <summary>
///     Handles Music & SFX
/// </summary>
public class AudioManager : PersistentSingleton<AudioManager>
{
    [Header("Audio Mixers")]
    [SerializeField] AudioMixer master = null;
    [SerializeField] AudioMixer music = null;
    [SerializeField] AudioMixer sfx = null;

    [SerializeField] public AudioClip planktonMusic;

    [Space]
    [Header("Default Fade Values")]
    [SerializeField] float fadeTime = 1f;



    public void PlaySFX(AudioSource audioSource, AudioClip audioClip)
    {

    }

    public void PlayRandomPitchSFX(AudioSource audioSource, AudioClip audioClip, float low = 0.75f, float high = 1.25f)
    {

    }

    public void StartMusic(AudioSource audioSource, AudioClip audioClip)
    {
         
    }

    public void FadeInMusic(AudioSource audioSource, AudioClip audioClip)
    {
        
    }
}