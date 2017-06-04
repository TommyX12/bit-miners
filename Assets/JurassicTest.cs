using UnityEngine;
using System;
using System.Collections;

public class JurassicTest : MonoBehaviour {
	
	// Use this for initialization
	void Start () {
		var scriptSystem = new ScriptSystem();
		scriptSystem.Script = @"
			print('Hello World!');
		";
		scriptSystem.Start(true);
		
		scriptSystem.Script = @"
			function f() {
				f();
			}
			
			f();
		";
		scriptSystem.Start(true);
		
		scriptSystem.Script = @"
			while (true);
		";
		scriptSystem.Start(true);
		
		scriptSystem.Script = @"
			function on_test(data) {
				print(data.message);
			}
		
			function on_test2(a, b) {
				print(a + b);
			}
		";
		scriptSystem.Start(true);
		var data = scriptSystem.ConstructObject();
		data["message"] = "Test Event Message";
		scriptSystem.DispatchEvent("none", 1, 2);
		scriptSystem.DispatchEvent("test2", "a", "b");
		scriptSystem.DispatchEvent("test", data);
		scriptSystem.DispatchEvent("test");
		scriptSystem.DispatchEvent("test", data);
		
		scriptSystem.Script = @"
			function _print() {
				
			}
		";
		scriptSystem.Start();
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
