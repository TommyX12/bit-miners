using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MyMono {
    
    public static GameManager Current;
    public ScriptEditor ScriptEditorObject;
    public ScriptEditorV2 ScriptEditorV2Object;
    public MapTile MapTilePrefab;
    public Unit Player;

    public bool Paused      = false;
    public bool HackEnabled = true;

    void Awake() {
        Current = this;
    }
    
    void Start() {
        
    }

    public override void PausingUpdate() {
        
    }
    
    public override void NormalUpdate() {
        if (PlayerMoveComponent.Current) {
            this.HackEnabled = PlayerMoveComponent.Current.isActiveAndEnabled;
        }
    }
    
    public override void PausingFixedUpdate() {
        
    }
    
    public override void NormalFixedUpdate() {
        
    }
}
