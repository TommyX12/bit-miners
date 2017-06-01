using UnityEngine;
using System;
using System.Collections;

public class JurassicTest : MonoBehaviour {
	
	public static string lib = @"
		
	";
	
	public static string test = @"
		function f(n) {
			if (n <= 0) return 1; 
			return n * f(n - 1);
		} 
		
		function poisson(x, l) {
			return Math.exp(-l) * Math.pow(l, x) / f(x);
		}
		
		print([poisson(4, 5), poisson(5, 5)]);
		print([fibonacci(0), fibonacci(1), fibonacci(2), fibonacci(3), fibonacci(4), fibonacci(5)]);
		print([host.string, host.int, host.float, host.bool, host.array]);
		print(host.properties);
		thing(host);
	";
	
	int fibonacci(int n) {
		if (n <= 1) return n;
		return fibonacci(n - 1) + fibonacci(n - 2);
	}
	
	void print(string str) {
		Debug.Log(str);
	}
	
	void thing(Jurassic.Library.ObjectInstance obj) {
		print(obj["string"]);
	}
	
	// Use this for initialization
	void Start () {
		var engine = new Jurassic.ScriptEngine();
		// engine.SetGlobalValue("console", new Jurassic.Library.FirebugConsole(engine));
		engine.SetGlobalFunction("print", new Action<string>(print));
		engine.Execute(lib);
		
		var host = engine.Object.Construct();
		host["string"] = "abc";
		host["int"] = 1;
		host["float"] = 3.14;
		host["bool"] = true;
		host["array"] = engine.Array.Construct(1, 2, 3, engine.Array.Construct(4, 5));
		host["properties"] = engine.Object.Construct();
		
		engine.SetGlobalValue("host", host);
		engine.SetGlobalFunction("fibonacci", new Func<int, int>(fibonacci));
		engine.SetGlobalFunction("thing", new Action<Jurassic.Library.ObjectInstance>(thing));
		
		engine.Execute(test);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
