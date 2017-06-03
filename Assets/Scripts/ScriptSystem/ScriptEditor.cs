using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptEditor : MyMono {
	
	public ScriptSystem CurrentScriptSystem {
		get; private set;
	}
	
	void Awake() {
		
	}

	void Start() {
		
	}
	
	public void StartEdit(ScriptSystem scriptSystem) {
		this.gameObject.SetActive(true);
		MyMono.Paused = true;
		
		this.CurrentScriptSystem = scriptSystem;
	}
	
	public void EndEdit() {
		this.gameObject.SetActive(false);
		MyMono.Paused = false;
		
		this.CurrentScriptSystem = null;
	}
	
	public override void NormalUpdate() {
		
	}
	
}
