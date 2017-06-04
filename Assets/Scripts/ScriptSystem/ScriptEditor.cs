using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScriptEditor : MyMono {
	
	public InputField InputFieldObject;
	
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
		
		InputFieldObject.text = this.CurrentScriptSystem.Script;
	}
	
	public void EndEdit() {
		this.gameObject.SetActive(false);
		MyMono.Paused = false;
		
		this.CurrentScriptSystem = null;
		
		InputFieldObject.text = "";
	}
	
	public override void NormalUpdate() {
		
	}
	
}
