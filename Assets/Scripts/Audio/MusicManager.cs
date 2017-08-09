using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using System.Linq;

public class MusicManager : MyMono {
    
    public static MusicManager Current;
    
    public class TrackConfig {
        public string name = null;
        public float volume = 1.0f;
        public bool synced = true;
        public bool loop = true;
        public float fade_in_time = 2.0f;
        public float fade_out_time = 2.0f;
        public float start_fade_in_time = 1.0f;
        
        public void Enable(float volumnMul) {
            MusicManager.Current.GetMusicTrack(this.name).Volumn = this.volume * volumnMul;
            MusicManager.Current.Play(this.name, this.loop, false, this.fade_in_time, this.start_fade_in_time);
        }
        public void Disable() {
            if (this.synced) {
                MusicManager.Current.Silence(this.name, this.fade_out_time);
            }
            else {
                MusicManager.Current.Stop(this.name, false, this.fade_out_time);
            }
        }
    }
    
    public class PatternConfig {
        public Dictionary<string, float> tracks = new Dictionary<string, float>();
        public List<List<string>> conditions = new List<List<string>>();
        
        public bool ConditionSatisfied() {
            foreach (var c in this.conditions) {
                bool passed = true;
                foreach (string d in c) {
                    if (!MusicManager.Current.GetCondition(d)) {
                        passed = false;
                        break;
                    }
                }
                
                if (passed) {
                    return true;
                }
            }
            
            return false;
        }
        
        public void PlayOnCondition() {
            if (this.ConditionSatisfied()) {
                foreach (var entry in this.tracks) {
                    string name = entry.Key;
                    float volumnMul = entry.Value;
                    MusicManager.Current.GetTrackConfigs()[name].Enable(volumnMul);
                }
            }
        }
    }
    
    public class Config {
        public List<TrackConfig> tracks = new List<TrackConfig>();
        public List<PatternConfig> patterns = new List<PatternConfig>();
    }
    
    private Dictionary<string, MusicTrack> activeTracks = new Dictionary<string, MusicTrack>();
    private GameObject audioSourceContainer;
    
    public AudioMixerGroup Mixer = null;
    public float DefaultFadeInTime = 2.0f;
    public float DefaultFadeOutTime = 2.0f;
    public float DefaultStartFadeInTime = 1.0f;
    
    public TextAsset DefaultConfigText;
    
    private Dictionary<string, TrackConfig> trackConfigs = null;
    private List<PatternConfig> patternConfigs = null;
    private Dictionary<string, bool> conditions = new Dictionary<string, bool>();
    
    private bool conditionsDirty = true;
    
    void Awake() {
        Current = this;
        this.audioSourceContainer = Util.MakeEmptyContainer(this.gameObject.transform);
        if (DefaultConfigText != null) {
            this.LoadConfig(Util.FromJson<Config>(this.DefaultConfigText.text));
        }
    }

    void Start() {
        
    }
    
    public Dictionary<string, TrackConfig> GetTrackConfigs() {
        return this.trackConfigs;
    }
    
    public void LoadConfig(string name) {
        this.LoadConfig(Util.FromJson<Config>(ResourceManager.GetText(name)));
    }
    
    public void LoadConfig(Config config) {
        this.trackConfigs = new Dictionary<string, TrackConfig>();
        foreach (TrackConfig trackConfig in config.tracks) {
            this.trackConfigs[trackConfig.name] = trackConfig;
        }
        
        this.patternConfigs = config.patterns;
        
        this.conditionsDirty = true;
    }
    
    public void SetCondition(string condition, bool value) {
        if (value == this.GetCondition(condition)) return;
        
        this.conditions[condition] = value;
        this.conditions["!" + condition] = !value;
        this.conditionsDirty = true;
    }
    
    public bool GetCondition(string condition) {
        if (!this.conditions.ContainsKey(condition)) return condition[0] == '!' ? true : false;
        
        return this.conditions[condition];
    }
    
    public MusicTrack AddMusicTrack(string name) {
        MusicTrack track = new MusicTrack(name, this.GetAudioClip(name), this.audioSourceContainer, this.Mixer);
        
        activeTracks.Add(name, track);
        return track;
    }
    
    public override void NormalFixedUpdate() {
        if (this.conditionsDirty) {
            if (this.trackConfigs != null && this.patternConfigs != null) {
                foreach (TrackConfig trackConfig in this.trackConfigs.Values) {
                    trackConfig.Disable();
                }
                foreach (PatternConfig patternConfig in this.patternConfigs) {
                    patternConfig.PlayOnCondition();
                }
            }
            
            this.conditionsDirty = false;
        }
        
        // update tracks
        foreach (var entry in this.activeTracks) {
            MusicTrack track = entry.Value;
            track.Update(Time.fixedDeltaTime);
        }
    }
    
    public bool HasMusicTrack(string name) {
        return this.activeTracks.ContainsKey(name);
    }
    
    public MusicTrack GetMusicTrack(string name) {
        if (!this.HasMusicTrack(name)) {
            return this.AddMusicTrack(name);
        }
        MusicTrack result;
        this.activeTracks.TryGetValue(name, out result);
        return result;
    }
    
    private AudioClip GetAudioClip(string name) {
        return ResourceManager.GetMusic(name);
    }
    
    public MusicTrack Play(string name, bool loop = true, bool forceRestart = false, float fadeInTime = -1, float startFadeInTime = -1) {
        if (fadeInTime < 0) fadeInTime = this.DefaultFadeInTime;
        if (startFadeInTime < 0) startFadeInTime = this.DefaultStartFadeInTime;
        
        MusicTrack track = this.GetMusicTrack(name);
        if (!forceRestart && track.Playing) {
            track.Resume(fadeInTime);
        }
        else {
            track.Start(startFadeInTime, loop);
        }
        return track;
    }
    
    public void Stop(string name, bool forceStop = false, float fadeOutTime = -1) {
        if (!this.HasMusicTrack(name)) return;
        
        if (fadeOutTime < 0) fadeOutTime = this.DefaultFadeOutTime;
        
        if (forceStop) {
            this.GetMusicTrack(name).ForceStop();
        }
        else {
            this.GetMusicTrack(name).Stop(fadeOutTime);
        }
    }
    
    public void Silence(string name, float fadeOutTime = -1) {
        if (!this.HasMusicTrack(name)) return;
        
        if (fadeOutTime < 0) fadeOutTime = this.DefaultFadeOutTime;
        
        this.GetMusicTrack(name).Silence(fadeOutTime);
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
