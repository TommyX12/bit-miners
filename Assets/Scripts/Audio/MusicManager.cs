using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MusicManager : MyMono {
    
    public static MusicManager Current;
    
    private Dictionary<string, MusicTrack> activeTracks = new Dictionary<string, MusicTrack>();
    private GameObject audioSourceContainer;

    void Awake() {
        Current = this;
        this.audioSourceContainer = Util.MakeEmptyContainer(this.gameObject.transform);
    }

    void Start() {
        
    }
    
    private MusicTrack AddMusicTrack(string name) {
        MusicTrack track = new MusicTrack(this.GetAudioClip(name), this.audioSourceContainer);
        
        activeTracks.Add(name, track);
        return track;
    }
    
    public override void NormalFixedUpdate() {
        foreach (var entry in this.activeTracks.ToArray()) {
            string name = entry.Key;
            MusicTrack track = entry.Value;
            track.Update();
            if (track.Finished) {
                this.activeTracks.Remove(name);
            }
        }
    }
    
    public bool HasMusicTrack(string name) {
        return this.activeTracks.ContainsKey(name);
    }
    
    private AudioClip GetAudioClip(string name) {
        return ResourceManager.GetMusic(name);
    }
    
    public void Play(string name) {
        if (!this.HasMusicTrack(name)) {
            this.AddMusicTrack(name);
        }
    }
    
    public void Stop(string name) {
        if (!this.HasMusicTrack(name)) return;
    }
    
    public void StopAll() {
        foreach (var name in this.activeTracks.Keys) {
            this.Stop(name);
        }
    }
    
}
