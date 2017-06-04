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
		
	";
	
	private const string postJavsScript = @"
		
	";
	
	public void Register(ScriptSystem scriptSystem) {
		scriptSystem.RegisterFunction("print", new Action<string>(Print));
		
		scriptSystem.RegisterJavaScript(javaScript);
		
		scriptSystem.RegisterMacro(
			@"\brepeat\s*" + ScriptSystem.MatchingBracketPattern("param"),
			@"for (var _ = 0; _ < ${param}; ++_)"
		);
		
		scriptSystem.RegisterMacro(
			@"\bwhen\s+" + ScriptSystem.IdentifierPattern("name") + ScriptSystem.MatchingBracketPattern("param"),
			@"function " + ScriptSystem.EVENT_HANDLER_PREFIX + @"${name}(${param})"
		);
		
		scriptSystem.RegisterMacro(
			@"\bloop\b",
			@"while (true)"
		);
	}
	
	public void PostRegister(ScriptSystem scriptSystem) {
		scriptSystem.RegisterJavaScript(postJavsScript);
	}
}
