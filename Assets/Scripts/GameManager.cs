using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MyMono {
	
	public ScriptEditor ScriptEditorObject;
	private ScriptSystem scriptSystem;
	
	void Awake() {
		this.scriptSystem = new ScriptSystem();
	}

	void Start() {
		
	}

	public override void PausingUpdate() {
		
	}
	
	public override void NormalUpdate() {
		if (Input.GetKeyDown(KeyCode.Tab)) {
			this.ScriptEditorObject.StartEdit(this.scriptSystem);
		}
	}
	
    public override void PausingFixedUpdate() {
		
    }
	
	public override void NormalFixedUpdate() {
		
	}
}
