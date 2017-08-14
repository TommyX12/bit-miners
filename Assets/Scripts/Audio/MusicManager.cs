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
        public bool play_on_awake = true;
        public bool loop = true;
        
        public float fade_in_time = -1;
        public float fade_out_time = -1;
        public float start_fade_in_time = -1;
        
        public void Init() {
            if (this.play_on_awake) {
                MusicManager.Current.Play(this.name, true, true, 999.0f, 999.0f);
                MusicManager.Current.Silence(this.name, 0.01f);
            }
        }
        
        public void Enable(float volumeMul, float fadeInTime = -1, float startFadeInTime = -1) {
            if (fadeInTime < 0) fadeInTime = this.fade_in_time;
            if (startFadeInTime < 0) startFadeInTime = this.start_fade_in_time;
            
            MusicManager.Current.GetMusicTrack(this.name, true).Volume = this.volume * volumeMul;
            MusicManager.Current.Play(this.name, this.loop, false, fadeInTime, startFadeInTime);
        }
        
        public void Disable(float fadeOutTime = -1) {
            if (fadeOutTime < 0) fadeOutTime = this.fade_out_time;
            
            if (this.synced) {
                MusicManager.Current.Silence(this.name, fadeOutTime);
            }
            else {
                MusicManager.Current.Stop(this.name, false, fadeOutTime);
            }
        }
    }
    
    public class SnapshotConfig {
        public Dictionary<string, float> tracks = new Dictionary<string, float>();
        public List<List<string>> conditions = new List<List<string>>();
        
        public float fade_in_time = -1;
        public float fade_out_time = -1;
        public float start_fade_in_time = -1;
        
        public bool ConditionSatisfied() {
            if (this.conditions.Count == 0) return true;
            
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
        
        public void Play() {
            foreach (var entry in this.tracks) {
                string name = entry.Key;
                float volumeMul = entry.Value;
                MusicManager.Current.GetTrackConfigs()[name].Enable(volumeMul, this.fade_in_time, this.start_fade_in_time);
            }
        }
    }
    
    public class Config {
        public List<TrackConfig> tracks = new List<TrackConfig>();
        public List<SnapshotConfig> snapshots = new List<SnapshotConfig>();
        public float fade_in_time = -1;
        public float fade_out_time = -1;
        public float start_fade_in_time = -1;
        public float bpm = 0;
    }
    
    private Dictionary<string, MusicTrack> activeTracks = new Dictionary<string, MusicTrack>();
    private GameObject audioSourceContainer;
    
    public AudioMixerGroup Mixer = null;
    public float DefaultFadeInTime = 1.5f;
    public float DefaultFadeOutTime = 2.5f;
    public float DefaultStartFadeInTime = 1.0f;
    
    public TextAsset DefaultConfigText;
    
    public float MasterVolume = 1.0f;
    public float ContextVolume = 1.0f;
    
    private Dictionary<string, TrackConfig> trackConfigs = null;
    private List<SnapshotConfig> snapshotConfigs = null;
    private Dictionary<string, bool> conditions = new Dictionary<string, bool>();
    private float beatLength = 0;
    private float beatTimer = 0;
    private float bpm = 0;
    public float CurrentBPM {
        get {
            return bpm;
        }
        set {
            if (value == 0) {
                this.beatLength = 0;
            }
            else {
                this.beatLength = 60 / value;
            }
            this.bpm = value;
        }
    }
    
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
            trackConfig.Init();
        }
        
        this.snapshotConfigs = config.snapshots;
        
        if (config.fade_in_time >= 0) this.DefaultFadeInTime = config.fade_in_time;
        if (config.fade_out_time >= 0) this.DefaultFadeOutTime = config.fade_out_time;
        if (config.start_fade_in_time >= 0) this.DefaultStartFadeInTime = config.start_fade_in_time;
        
        this.conditionsDirty = true;
        
        this.beatTimer = 0;
        this.CurrentBPM = config.bpm;
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
        bool onBeat = false;
        if (this.beatTimer >= this.beatLength) {
            onBeat = true;
            this.beatTimer = this.beatTimer % this.beatLength;
        }
        this.beatTimer += Time.fixedDeltaTime;
        
        if (this.conditionsDirty && onBeat) {
            if (this.trackConfigs != null && this.snapshotConfigs != null) {
                float fadeOutTime = -1;
                SnapshotConfig nextSnapshot = null;
                foreach (SnapshotConfig snapshotConfig in this.snapshotConfigs) {
                    if (snapshotConfig.ConditionSatisfied()) {
                        nextSnapshot = snapshotConfig;
                        fadeOutTime = snapshotConfig.fade_out_time;
                        break;
                    }
                }
                foreach (TrackConfig trackConfig in this.trackConfigs.Values) {
                    trackConfig.Disable(fadeOutTime);
                }
                if (nextSnapshot != null) {
                    nextSnapshot.Play();
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
    
    public MusicTrack GetMusicTrack(string name, bool initIfNotExist = false) {
        if (initIfNotExist && !this.HasMusicTrack(name)) {
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
        
        MusicTrack track = this.GetMusicTrack(name, true);
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
