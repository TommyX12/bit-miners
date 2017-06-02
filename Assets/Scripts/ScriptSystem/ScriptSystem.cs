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
	
	public ScriptSystem(List<IScriptSystemAPI> apiList = null) {
		this.APIList = apiList == null ? new List<IScriptSystemAPI>() : apiList;
		this.APIList.Insert(0, BasicAPI.GetInstance());
		this.Running = false;
		this.Script = "";
		
		this.timeoutHelper = new ScriptTimeoutHelper();
	}
	
	public void RegisterFunction(string functionName, Delegate functionDelegate) {
		string wrapperArguments = "";
		int numArguments = functionDelegate.Method.GetParameters().Length;
		for (int i = 0; i < numArguments; ++i) {
			if (i > 0) wrapperArguments += ", ";
			wrapperArguments += "a" + i;
		}
		string wrapperFunction = @"
			function " + functionName + "(" + wrapperArguments + @") {
				_critical_on();
				var result = __" + functionName +"__(" + wrapperArguments + @");
				_critical_off();
				
				return result;
			}
		";
		this.engine.SetGlobalFunction("__" + functionName + "__", functionDelegate);
		this.engine.Execute(wrapperFunction);
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
		this.engine.SetGlobalFunction("_critical_on", new Action(this.timeoutHelper.EnterCriticalSection));
		this.engine.SetGlobalFunction("_critical_off", new Action(this.timeoutHelper.ExitCriticalSection));
		foreach (IScriptSystemAPI api in this.APIList) {
			api.Register(this);
		}
		
		// Execution
		this.ExecuteAction(() => {
			this.engine.Execute(this.Script);
		});
		
		// API Post registration
		foreach (IScriptSystemAPI api in this.APIList) {
			api.PostRegister(this);
		}
		
		// Status update
		this.Running = true;
	}
	
	public Jurassic.Library.ObjectInstance ConstructObject() {
		if (!this.Running) return null;
		
		return this.engine.Object.Construct();
	}
	
	public void DispatchEvent(string name, Jurassic.Library.ObjectInstance data = null) {
		if (!this.Running) return;
		
		this.ExecuteAction(() => {
			if (data == null) {
				this.engine.CallGlobalFunction("_dispatch", name);
			}
			else {
				this.engine.CallGlobalFunction("_dispatch", name, data);
			}
		});
	}
	
	private void ExecuteAction(Action action) {
		try {
			this.timeoutHelper.RunWithTimeout(action, this.TimeoutMS);
		}
		catch (TimeoutException ex) {
			Debug.LogWarning("TimeoutException");
			this.Running = false;
		}
		catch (OutOfMemoryException ex) {
			Debug.LogWarning("OutOfMemoryException");
			this.Running = false;
		}
		catch (StackOverflowException ex) {
			Debug.LogWarning("StackOverflowException");
			this.Running = false;
		}
		catch (Jurassic.JavaScriptException ex) {
			Debug.LogWarning(ex.Message);
			this.Running = false;
		}
	}
	
}
