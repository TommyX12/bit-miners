using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class ScriptSystem {
	
	private Jurassic.ScriptEngine engine;
	
	public List<IScriptSystemAPI> APIList {
		get; set;
	}
	
	public bool Running {
		get; private set;
	}
	
	public string Script {
		get; set;
	}
	
	public ScriptSystem(List<IScriptSystemAPI> apiList) {
		this.APIList = apiList;
		this.Running = false;
		this.Script = "";
		
		this.engine = new Jurassic.ScriptEngine();
	}
	
	public void RegisterFunction(string functionName, Delegate functionDelegate) {
		this.engine.SetGlobalFunction(functionName, functionDelegate);
	}
	
	public void RegisterVariable(string variableName, object variableValue) {
		this.engine.SetGlobalValue(variableName, variableValue);
	}
	
	public void RegisterJavaScript(string script) {
		this.engine.Execute(script);
	}
	
	public void Start() {
		// API registration
		BasicAPI.GetInstance().Register(this);
		foreach (IScriptSystemAPI api in this.APIList) {
			api.Register(this);
		}
		
		this.engine.Execute(this.Script);
		
		this.Running = true;
	}
	
	public void DispatchEvent() {
		if (!this.Running) return;
	}
	
}
