using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MyMono {
	
	public ScriptEditor ScriptEditorObject;
	private ScriptSystem scriptSystem;
	
	void Awake() {
		this.scriptSystem = new ScriptSystem();
		this.scriptSystem.Script = @"
function hello_world() {
	print('Hello, World!')
}

hello_world()

when start() {
	for (var i) from (1) to (5) by (2) {
		var array = []
		for (var j) from (i) to (0) {
			array.push(j)
		}
		print(array)
	}
}

when update() {
	
}
";
		this.scriptSystem.Start();
	}

	void Start() {
		
	}

	public override void PausingUpdate() {
		this.scriptSystem.DispatchEvent("update");
	}
	
	public override void NormalUpdate() {
		if (Input.GetKeyDown(KeyCode.Tab)) {
			if (this.ScriptEditorObject.ScriptSystemObject == null) {
				this.ScriptEditorObject.StartEdit(this.scriptSystem);
			}
		}
	}
	
    public override void PausingFixedUpdate() {
		
    }
	
	public override void NormalFixedUpdate() {
		
	}
}
