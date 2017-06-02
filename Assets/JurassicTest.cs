using UnityEngine;
using System;
using System.Collections;

public class JurassicTest : MonoBehaviour {
	
	// Use this for initialization
	void Start () {
		var scriptSystem = new ScriptSystem();
		scriptSystem.Script = @"
			_print('Hello World!');
		";
		scriptSystem.Start();
		
		scriptSystem.Script = @"
			function f() {
				f();
			}
			
			f();
		";
		scriptSystem.Start();
		
		scriptSystem.Script = @"
			while (true);
		";
		scriptSystem.Start();
		
		scriptSystem.Script = @"
			when('test', function(data) {
				_print(data.message);
			})
		";
		scriptSystem.Start();
		var data = scriptSystem.ConstructObject();
		data["message"] = "Test Event Message";
		scriptSystem.DispatchEvent("test", data);
		scriptSystem.DispatchEvent("test");
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
