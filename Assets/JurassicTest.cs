using UnityEngine;
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
	
		[Poisson(4, 5), Poisson(5, 5)].toString();
	";

	// Use this for initialization
	void Start () {
		Debug.Log("hello?");
		var engine = new Jurassic.ScriptEngine();
		Debug.Log(engine.Evaluate(test));
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
