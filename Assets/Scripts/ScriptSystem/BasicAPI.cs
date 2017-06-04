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
	
	private void Print(string str) {
		Debug.Log(str);
	}
	
	private const string javaScript = @"
		/* var _events = {};
		var _events_sealed = false;
		
		// cannot call this function inside other function. only when initializing.
		function when(event, handler) {
			if (_events_sealed) return;
			
			if (_events[event] === undefined) {
				_events[event] = [];
			}
			
			_events[event].push(handler);
		}
	
		function _dispatch(event, data) {
			if (_events[event] === undefined) return;
			
			var handlers = _events[event];
			for (var i = 0; i < handlers.length; ++i) {
				var handler = handlers[i];
				handler(data);
			}
		} */
	";
	
	private const string postJavsScript = @"
		// _events_sealed = true;
	";
	
	public void Register(ScriptSystem scriptSystem) {
		scriptSystem.RegisterFunction("print", new Action<string>(Print));
		
		scriptSystem.RegisterJavaScript(javaScript);
	}
	
	public void PostRegister(ScriptSystem scriptSystem) {
		scriptSystem.RegisterJavaScript(postJavsScript);
	}
}
