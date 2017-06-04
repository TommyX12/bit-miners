using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScriptEditor : MyMono {
	
	public InputField InputFieldObject;
	public Button SaveButtonObject;
	public Button DiscardButtonObject;
	
	public ScriptSystem CurrentScriptSystem {
		get; private set;
	}
	
	void Awake() {
		this.DiscardButtonObject.onClick.AddListener(this.DiscardEdit);
	}

	void Start() {
		
	}
	
	public void StartEdit(ScriptSystem scriptSystem) {
		this.gameObject.SetActive(true);
		MyMono.Paused = true;
		
		this.CurrentScriptSystem = scriptSystem;
		
		this.InputFieldObject.text = this.CurrentScriptSystem.Script;
	}
	
	public void DiscardEdit() {
		EndEdit();
	}
	
	public void SaveEdit() {
		
	}
	
	private void EndEdit() {
		this.gameObject.SetActive(false);
		MyMono.Paused = false;
		
		this.CurrentScriptSystem = null;
		
		InputFieldObject.text = "";
	}
	
	public override void NormalUpdate() {
		
	}
	
}
