using UnityEngine;
using System;

public class BasicAPI : IScriptSystemAPI {
	
	private static BasicAPI instance;
	public static BasicAPI GetInstance() {
		if (instance == null) {
			instance = new BasicAPI();
		}
		
		return instance;
	}
		
	private BasicAPI() {
		
	}
	
	private void print(string str) {
		Debug.Log(str);
	}
	
	private static string javaScript = @"
		var _events = {};
		
		function when(event, handler) {
			if (_events[event] === undefined) {
				_events[event] = [];
			}
			
			_events[event].push(handler);
		}
	
		function dispatch(event, data) {
			if (_events[event] === undefined) return;
			
			var handlers = _events[event];
			for (var i = 0; i < handlers.length; ++i) {
				var handler = handlers[i];
				handler(data);
			}
		}
	";
	
	public void Register(ScriptSystem scriptSystem, Jurassic.ScriptEngine engine) {
		scriptSystem.RegisterFunction("print", new Action<string>(print));
		
		scriptSystem.RegisterJavaScript(javaScript);
	}
}
