using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

/// <summary>
///     Handles Music & SFX
///     Centralized clips 
/// </summary>
///

public class AudioManager : PersistentSingleton<AudioManager>
{
    [Header("Audio Mixers")]
    [SerializeField] AudioMixer master = null;
    [SerializeField] AudioMixerGroup music = null;
    [SerializeField] AudioMixerGroup sfx = null;

    [Space]
    [Header("AudioSource")]
    [SerializeField] public AudioSource _source;
    [SerializeField] public AudioSource _sourceMusic;
    [SerializeField] public AudioSource _sourceSFX;


    [Header("Music Audio Clips")]
    // One time
    [SerializeField] public AudioClip storyMusic;
    // Loops
    [SerializeField] public AudioClip planktonMusic;
    [SerializeField] public AudioClip trashyTroubleMusic;
    [SerializeField] public AudioClip overFishingMusic;
    [SerializeField] public AudioClip coralCleaningMusic;

    [Header("SFX Audio Clips")]
    [SerializeField] public AudioClip button1;
    [SerializeField] public AudioClip button2;

    [Space]
    [Header("Default Fade Values")]
    [SerializeField] float fadeTime = 1f;



    public void PlaySFX(AudioSource _source, AudioClip audioClip)
    {
        _source.clip = audioClip; 
        _source.volume = 1.0f; 
        _source.Play();
    }

    public void PlayRandomPitchSFX(AudioSource _source, AudioClip audioClip, float low = 0.75f, float high = 1.25f)
    {
        _source.clip = audioClip;
        _source.pitch = Random.Range(0.7f, 1.25f);
        _source.Play(); 
    }

    public void StartAmbienceLoop(AudioSource _source, AudioClip audioClip)
    {
        _source.clip = audioClip; 
        _source.loop = true;
        _source.volume = 0.4f; 
        Debug.Log("Background Ambience Playing");
        _source.Play();
    }

    public void PlayMusic(AudioSource _source, AudioClip audioClip)
    {
        _source.clip = audioClip; 
        _source.loop = true;
        _source.volume = 0.6f; 
        Debug.Log("Music Playing"); 
        _source.Play();

    }

    public void FadeMusic(AudioSource _source, AudioClip audioClip)
    {
        
    }
}