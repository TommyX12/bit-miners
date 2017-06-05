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
		function _closer_to(i, from, to) {
			if (i > to) --i;
			else if (i < to) ++i;
			else {
				if (to > from) ++i;
				else --i;
			}
			return i;
		}
	
		function _in_between(i, a, b) {
			if (a < b) {
				return a <= i && i <= b;
			}
			else {
				return b <= i && i <= a;
			}
		}
	";
	
	private const string postJavsScript = @"
		
	";
	
	public void Register(ScriptSystem scriptSystem) {
		scriptSystem.RegisterFunction("print", new Action<string>(Print));
		
		scriptSystem.RegisterJavaScript(javaScript);
		
		/* scriptSystem.RegisterMacro(
			@"\brepeat\s*" + ScriptSystem.MatchingBracketPattern("param"),
			@"for (var _ = 0, print('hi'); _ < ${param}; ++_)"
		); */
		
		scriptSystem.RegisterMacro(
			@"\bwhen\s+" + ScriptSystem.IdentifierPattern("name") + ScriptSystem.MatchingBracketPattern("param"),
			@"function " + ScriptSystem.EVENT_HANDLER_PREFIX + @"${name}(${param})"
		);
		
		scriptSystem.RegisterMacro(
			@"\bfor\s+\(\s*var\s+" + ScriptSystem.IdentifierPattern("var") + @"\s*\)\s+from\s+" + ScriptSystem.MatchingBracketPattern("from") + @"\s+to\s+" + ScriptSystem.MatchingBracketPattern("to") + @"\s+by\s+" + ScriptSystem.MatchingBracketPattern("by"),
			@"for (var ${var} = ${from}; _in_between(${var}, ${from}, ${to}); ${var} += ${by})"
		);
		
		scriptSystem.RegisterMacro(
			@"\bfor\s+\(\s*var\s+" + ScriptSystem.IdentifierPattern("var") + @"\s*\)\s+from\s+" + ScriptSystem.MatchingBracketPattern("from") + @"\s+to\s+" + ScriptSystem.MatchingBracketPattern("to"),
			@"for (var ${var} = ${from}; _in_between(${var}, ${from}, ${to}); ${var} = _closer_to(${var}, ${from}, ${to}))"
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
