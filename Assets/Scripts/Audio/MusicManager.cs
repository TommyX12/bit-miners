using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using System.Linq;

public class MusicManager : MyMono {
    
    public static MusicManager Current;
    
    private Dictionary<string, MusicTrack> activeTracks = new Dictionary<string, MusicTrack>();
    private GameObject audioSourceContainer;
    
    public AudioMixerGroup Mixer = null;
    public float FadeInTime = 2.0f;
    public float FadeOutTime = 2.0f;
    public float StartFadeInTime = 1.0f;
    
    void Awake() {
        Current = this;
        this.audioSourceContainer = Util.MakeEmptyContainer(this.gameObject.transform);
    }

    void Start() {
        
    }
    
    public MusicTrack AddMusicTrack(string name) {
        MusicTrack track = new MusicTrack(name, this.GetAudioClip(name), this.audioSourceContainer, this.Mixer);
        
        activeTracks.Add(name, track);
        return track;
    }
    
    public override void NormalFixedUpdate() {
        foreach (var entry in this.activeTracks) {
            MusicTrack track = entry.Value;
            // string name = entry.Key;
            track.Update(Time.fixedDeltaTime);
        }
    }
    
    public bool HasMusicTrack(string name) {
        return this.activeTracks.ContainsKey(name);
    }
    
    public MusicTrack GetMusicTrack(string name) {
        MusicTrack result;
        this.activeTracks.TryGetValue(name, out result);
        return result;
    }
    
    private AudioClip GetAudioClip(string name) {
        return ResourceManager.GetMusic(name);
    }
    
    public MusicTrack Play(string name, bool loop = true, bool forceRestart = false) {
        MusicTrack track = null;
        if (!this.HasMusicTrack(name)) {
            track = this.AddMusicTrack(name);
        }
        else {
            track = this.GetMusicTrack(name);
        }
        if (!forceRestart && track.Playing) {
            track.Resume(this.FadeInTime);
        }
        else {
            track.Start(this.StartFadeInTime, loop);
        }
        return track;
    }
    
    public void Stop(string name, bool forceStop = false) {
        if (!this.HasMusicTrack(name)) return;
        
        if (forceStop) {
            this.GetMusicTrack(name).ForceStop();
        }
        else {
            this.GetMusicTrack(name).Stop(this.FadeOutTime);
        }
    }
    
    public void Silence(string name) {
        if (!this.HasMusicTrack(name)) return;
        
        this.GetMusicTrack(name).Silence(this.FadeOutTime);
    }
    
    public void StopAll(bool forceStop = false) {
        foreach (var name in this.activeTracks.Keys) {
            this.Stop(name, forceStop);
        }
    }
    
    public void SilenceAll() {
        foreach (var name in this.activeTracks.Keys) {
            this.Silence(name);
        }
    }
    
}
