using UnityEngine;
using System;
using System.Runtime.ExceptionServices;
using System.Collections;
using System.Collections.Generic;

public class ScriptSystem {
	
	private Jurassic.ScriptEngine engine;
	private ScriptTimeoutHelper timeoutHelper;
	
	public int TimeoutMS = 100;
	public int RecursionDepthLimit = 1000;
	
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
		
		this.timeoutHelper = new ScriptTimeoutHelper();
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
		this.engine = new Jurassic.ScriptEngine();
		this.engine.RecursionDepthLimit = this.RecursionDepthLimit;
		
		// API registration
		BasicAPI.GetInstance().Register(this, this.engine);
		foreach (IScriptSystemAPI api in this.APIList) {
			api.Register(this, this.engine);
		}
		
		// Execution
		this.ExecuteAction(() => {
			this.engine.Execute(this.Script);
		});
		
		// Status update
		this.Running = true;
	}
	
	public void DispatchEvent() {
		if (!this.Running) return;
	}
	
	private void ExecuteAction(Action action) {
		try {
			this.timeoutHelper.RunWithTimeout(action, this.TimeoutMS);
		}
		catch (TimeoutException ex) {
			
		}
		catch (OutOfMemoryException ex) {
			
		}
		catch (StackOverflowException ex) {
			
		}
		catch (Jurassic.JavaScriptException ex) {
			
		}
	}
	
}
