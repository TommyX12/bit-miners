using UnityEngine;
using System;
using System.Runtime.ExceptionServices;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class ScriptSystem {
	
	private Jurassic.ScriptEngine engine;
	private ScriptTimeoutHelper timeoutHelper;
	
	public int TimeoutMS = 100;
	public int RecursionDepthLimit = 1000;
	
	// the instance that is currently executing a script.
	// used to returns an object constructed by the instance currently executing a script, used by c# functions callable in script.
	public static ScriptSystem Current {
		get; private set;
	}
	
	public static string MatchingBracketPattern(string groupName) {
		return @"
			\(                      # First '('
				(?<" + groupName + @">
				(?:
				[^()]               # Match all non-braces
				|
				(?<br> \( )         # Match '(', and capture into 'br'
				|
				(?<-br> \) )        # Match ')', and delete the 'br' capture
				)*
				(?(br)(?!))         # Fails if 'br' stack isn't empty!
				)
			\)                      # Last ')'
		";
	}
	
	public static string IdentifierPattern(string groupName) {
		return @"(?<" + groupName + ">[a-zA-Z_][a-zA-Z0-9_]*)";
	}
	
	public const string EVENT_HANDLER_PREFIX = "_on_";
	
	public List<IScriptSystemAPI> APIList {
		get; set;
	}
	
	public bool Running {
		get; private set;
	}
	
	public bool ErrorCaught {
		get; private set;
	}
	
	public string Script {
		get; set;
	}
	
	public string Message {
		get; private set;
	}
	
	private List<Macro> macros;
	
	public ScriptSystem(List<IScriptSystemAPI> apiList = null) {
		this.APIList = apiList == null ? new List<IScriptSystemAPI>() : apiList;
		this.APIList.Insert(0, BasicAPI.GetInstance());
		this.Running = false;
		this.ErrorCaught = false;
		this.Script = "";
		this.Message = "Idle";
		
		this.timeoutHelper = new ScriptTimeoutHelper();
	}
	
	public void AddToList(string apiElement) {
		
	}
	
	public void RegisterFunction(string functionName, Delegate functionDelegate, bool unlisted = false) {
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
		
		if (!unlisted) this.AddToList(functionName);
	}
	
	public void RegisterVariable(string variableName, object variableValue) {
		this.engine.SetGlobalValue(variableName, variableValue);
	}
	
	public void RegisterJavaScript(string script) {
		this.engine.Execute(script);
	}
	
	public void RegisterMacro(string pattern, string replacement) {
		this.macros.Add(new Macro(pattern, replacement));
	}
	
	public void Start(bool skipValidation = false) {
		// Validation
		if (!skipValidation && !this.ValidateScript()) {
			Debug.LogWarning("User script contains elements not allowed for stability reasons");
			return;
		}
		
		// Initialization
		this.engine = new Jurassic.ScriptEngine();
		this.engine.RecursionDepthLimit = this.RecursionDepthLimit;
		this.macros = new List<Macro>();
		
		// API registration
		this.engine.SetGlobalFunction("_critical_on", new Action(this.timeoutHelper.EnterCriticalSection));
		this.engine.SetGlobalFunction("_critical_off", new Action(this.timeoutHelper.ExitCriticalSection));
		foreach (IScriptSystemAPI api in this.APIList) {
			api.Register(this);
		}
		
		// Macros registration
		string compiledScript = this.Script;
		foreach (Macro macro in this.macros) {
			compiledScript = Regex.Replace(compiledScript, macro.Pattern, macro.Replacement, RegexOptions.IgnorePatternWhitespace);
		}
		
		// Status update
		this.Running = true;
		this.ErrorCaught = false;
		this.Message = "Running";
		
		// Execution
		this.ExecuteAction(() => {
			this.engine.Execute(compiledScript);
		});
		if (this.ErrorCaught) return;
		this.DispatchEvent("start");
		
		// API Post registration
		foreach (IScriptSystemAPI api in this.APIList) {
			api.PostRegister(this);
		}
	}
	
	public bool ValidateScript() {
		string pattern = @"\b_\w*\b";
		Regex regex = new Regex(pattern);
		Match match = regex.Match(this.Script);
		if (match.Success) return false;
		
		return true;
	}
	
	public Jurassic.Library.ObjectInstance ConstructObject() {
		if (!this.Running) return null;
		
		return this.engine.Object.Construct();
	}
	
	public void DispatchEvent(string name, params object[] args) {
		if (!this.Running) return;
		
		string handlerName = EVENT_HANDLER_PREFIX + name;
		
		if (this.engine.Evaluate<bool>("this[\'" + handlerName + "\'] === undefined")) return;
		
		this.ExecuteAction(() => {
			this.engine.CallGlobalFunction(handlerName, args);
		});
	}
	
	private void ExecuteAction(Action action) {
		try {
			Current = this;
			this.timeoutHelper.RunWithTimeout(action, this.TimeoutMS);
		}
		catch (Exception ex) {
			this.Running = false;
			this.ErrorCaught = true;
			
			if (ex is TimeoutException) {
				this.Message = "Time limit exceeded";
			}
			else if (ex is OutOfMemoryException) {
				this.Message = "Out of memory";
			}
			else if (ex is StackOverflowException) {
				this.Message = "Recursion depth limit exceeded";
			}
			else if (ex is Jurassic.JavaScriptException) {
				Jurassic.JavaScriptException jsex = (Jurassic.JavaScriptException)ex;
				this.Message = "Script error: Line " + jsex.LineNumber + " - " + jsex.ErrorObject.ToString();
			}
			else {
				throw;
			}
			
			Debug.LogWarning(this.Message);
		}
		finally {
			Current = null;
		}
	}
	
	private struct Macro {
		public string Pattern;
		public string Replacement;
		
		public Macro(string pattern, string replacement) {
			this.Pattern = pattern;
			this.Replacement = replacement;
		}
	}
	
}
