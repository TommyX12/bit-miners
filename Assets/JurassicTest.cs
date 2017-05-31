using UnityEngine;
using System;
using System.Collections;

public class JurassicTest : MonoBehaviour {
	
	public static string test = @"
		function f(n) {
			if (n <= 0) return 1; 
			return n * f(n - 1);
		} 
		
		function Poisson(x, l) {
			return Math.exp(-l) * Math.pow(l, x) / f(x);
		}
	
		// [Poisson(4, 5), Poisson(5, 5)].toString();
		[fibonacci(0), fibonacci(1), fibonacci(2), fibonacci(3), fibonacci(4), fibonacci(5)].toString();
	";
	
	int fibonacci(int n) {
		if (n <= 1) return n;
		return fibonacci(n - 1) + fibonacci(n - 2);
	}

	// Use this for initialization
	void Start () {
		Debug.Log("hello?");
		var engine = new Jurassic.ScriptEngine();
		engine.SetGlobalFunction("fibonacci", new Func<int, int>(fibonacci));
		Debug.Log(engine.Evaluate(test));
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
