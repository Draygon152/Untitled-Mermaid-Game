using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

/// <summary>
///     Handles Music & SFX
///     Centralized UI clips 
/// </summary>
///

public class AudioManager : PersistentSingleton<AudioManager>
{
    [Header("Mixer References")]
    [SerializeField] private AudioMixer master = null;
    private const string MASTER_VOL = "masterVol";
    private const string MUSIC_VOL = "musicVol";
    private const string SFX_VOL = "sfxVol";

    [Header("AudioSource")]
    [SerializeField] public AudioSource _source;
    [SerializeField] public AudioSource _sourceMusic;
    [SerializeField] public AudioSource _sourceSFX;

    [Header("Music Audio Clips")]
    
    [SerializeField] public AudioClip menuMusic;
    [SerializeField] public AudioClip endMusic;

    // Loops
    [SerializeField] public AudioClip planktonMusic;
    [SerializeField] public AudioClip trashyTroubleMusic;
    [SerializeField] public AudioClip overFishingMusic;
    [SerializeField] public AudioClip coralCleaningMusic;

    [Header("SFX Audio Clips")]
    [SerializeField] public AudioClip button1;
    [SerializeField] public AudioClip button2;
    [SerializeField] public AudioClip buttonMiniGame; 
    [SerializeField] public AudioClip buttonDialogue;
    [SerializeField] public AudioClip buttonHover; 

    [Space]
    [Header("Default Fade Values")]
    [SerializeField] float fadeTime = 1f;


    private void Start()
    {
        LoadAudioSettings();
    }

    public void PlaySFX(AudioSource _source, AudioClip audioClip)
    {
        _source.clip = audioClip; 
        _source.volume = 1.0f; 
        _source.Play();
    }

    public void PlaySFXLoop(AudioSource _source, AudioClip audioClip)
    {
        _source.clip = audioClip;
        _source.volume = 1.0f;
        _source.loop = true;
        _source.Play(); 
    }

    public void StopSFXLoop(AudioSource _source, AudioClip audioClip)
    {
        _source.clip = audioClip;
        _source.volume = 0.7f; 
        _source.Stop(); 
    }

    public void PlayRandomPitchSFX(AudioSource _source, AudioClip audioClip, float low = 0.75f, float high = 1.25f)
    {
        // Only for the dialogue clicks 
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

    private void LoadAudioSettings()
    {
        LoadMasterVolume();
        LoadSFXVolume();
        LoadMusicVolume();
    }

    public void LoadMasterVolume()
    {
        master.SetFloat(MASTER_VOL, Mathf.Log(SaveDataUtility.LoadFloat(SaveDataUtility.MASTER_VOLUME)) * 20);
    }

    public void LoadSFXVolume()
    {
        master.SetFloat(SFX_VOL, Mathf.Log(SaveDataUtility.LoadFloat(SaveDataUtility.SFX_VOLUME)) * 20);
    }

    public void LoadMusicVolume()
    {
        master.SetFloat(MUSIC_VOL, Mathf.Log(SaveDataUtility.LoadFloat(SaveDataUtility.MUSIC_VOLUME)) * 20);
    }
}