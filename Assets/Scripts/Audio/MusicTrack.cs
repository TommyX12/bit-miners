using System;
using System.Collections.Generic;
using UnityEngine;

public class MusicTrack {
    
    public bool Finished = false;
    
    private AudioSource audioSource;
    
    public MusicTrack(AudioClip clip, GameObject audioSourceContainer) {
        this.audioSource = this.CreateAudioSource(clip, audioSourceContainer);
    }
    
    public AudioSource CreateAudioSource(AudioClip clip, GameObject audioSourceContainer) {
        AudioSource audioSource = audioSourceContainer.AddComponent<AudioSource>();
        
        return audioSource;
    }
    
    public void Update() {
        
    }
    
    public void Play() {
        
    }
    
    public void Stop() {
        
    }
    
}
