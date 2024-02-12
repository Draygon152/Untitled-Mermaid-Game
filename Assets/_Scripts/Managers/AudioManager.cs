using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AudioManager : LLPersistentSingleton<AudioManager>  
{
    // Handles Music & SFX

    
    //Singleton

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void StartMusic(AudioSource audioSource, AudioClip audioClip)
    {
        AudioClip.Play;
        audioSource.loop = true
        Loop = true; 
    }
}
