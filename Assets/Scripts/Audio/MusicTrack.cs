using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MusicTrack {
    
    public string Name {
        get; private set;
    }
    
    public bool Playing {
        get {
            return this.audioSource.isPlaying;
        }
    }
    
    public bool Stopping {
        get; private set;
    }
    
    public float Volume = 1.0f;
    private float volumeMul = 0.0f;
    private float volumeMulFade = 0.0f;
    
    private AudioSource audioSource;
    
    public MusicTrack(string name, AudioClip clip, GameObject audioSourceContainer, AudioMixerGroup mixer) {
        this.audioSource = this.CreateAudioSource(clip, audioSourceContainer, mixer);
        this.Name        = name;
        
        this.Stopping    = false;
    }
    
    public AudioSource CreateAudioSource(AudioClip clip, GameObject audioSourceContainer, AudioMixerGroup mixer) {
        AudioSource audioSource = audioSourceContainer.AddComponent<AudioSource>();
        audioSource.clip = clip;
        audioSource.outputAudioMixerGroup = mixer;
        audioSource.bypassEffects = true;
        audioSource.bypassListenerEffects = true;
        audioSource.bypassReverbZones = true;
        audioSource.dopplerLevel = 0;
        audioSource.playOnAwake = false;
        audioSource.reverbZoneMix = 0;
        audioSource.spatialize = false;
        
        return audioSource;
    }
    
    public void Update(float deltaTime) {
        this.volumeMul = Util.Clamp(
            this.volumeMul + deltaTime * this.volumeMulFade,
            0, 1
        );
        if (this.Playing && this.Stopping && this.volumeMul <= 0) {
            this.audioSource.Stop();
        }
        this.UpdateVolume();
    }
    
    public void UpdateVolume() {
        this.audioSource.volume = 
            MusicManager.Current.MasterVolume * 
            MusicManager.Current.ContextVolume * 
            this.Volume * this.volumeMul;
    }
    
    public void Start(float fadeInTime, bool loop) {
        this.audioSource.loop = loop;
        this.audioSource.Play();
        
        this.Stopping = false;
        this.volumeMul = 0;
        this.SetFade(true, fadeInTime);
        this.Update(0);
    }
    
    public void Stop(float fadeOutTime) {
        if (!this.Playing) return;
        this.Stopping = true;
        this.SetFade(false, fadeOutTime);
        this.Update(0);
    }
    
    public void Resume(float fadeInTime) {
        if (!this.Playing) return;
        this.Stopping = false;
        this.SetFade(true, fadeInTime);
        this.Update(0);
    }
    
    public void Silence(float fadeOutTime) {
        if (!this.Playing) return;
        this.Stopping = false;
        this.SetFade(false, fadeOutTime);
        this.Update(0);
    }
    
    private void SetFade(bool fadeIn, float fadeTime) {
        this.volumeMulFade = (fadeIn ? 1 : -1) / (fadeTime == 0 ? 0.0001f : fadeTime);
    }
    
    public void ForceStop() {
        this.audioSource.Stop();
    }
    
}
