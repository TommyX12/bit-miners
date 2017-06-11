using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MyMono {
	
	public static GameManager Current;
	public ScriptEditor ScriptEditorObject;
	public MapTile MapTilePrefab;
	
	void Awake() {
		Current = this;
	}

	void Start() {
		
	}

	public override void PausingUpdate() {
		
	}
	
	public override void NormalUpdate() {
		
	}
	
    public override void PausingFixedUpdate() {
		
    }
	
	public override void NormalFixedUpdate() {
		
	}
}
