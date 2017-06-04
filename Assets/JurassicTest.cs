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
			loop;
		";
		scriptSystem.Start(true);
		
		scriptSystem.Script = @"
			when test(data) {
				print(data.message);
			}
		
			function nothing(a) {
				return 0;
			}
		
			when test2(a, b) {
				repeat (b + nothing(nothing())) { nothing()
					print(a);
				}
			}
		";
		scriptSystem.Start(true);
		var data = scriptSystem.ConstructObject();
		data["message"] = "Test Event Message";
		scriptSystem.DispatchEvent("none", 1, 2);
		scriptSystem.DispatchEvent("test2", "a", 5);
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
