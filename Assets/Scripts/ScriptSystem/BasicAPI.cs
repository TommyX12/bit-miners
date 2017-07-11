using UnityEngine;
using System.Collections.Generic;
using System;

public class BasicAPI : IScriptSystemAPI {
    
    private static BasicAPI instance;
    public static BasicAPI GetInstance() {
        if (instance == null) {
            instance = new BasicAPI();
        }
        
        return instance;
    }
    
    private static List<SEBlockDef> blockDefs = new List<SEBlockDef>() {
        new SEBlockDef(){
            Name = "function",
            CursorIndex = 0,
            Flags = SEBlockDef.F_DEFINITION,
            Type = "definition",
            Elements = new SEElementDef[]{
                new SEElementDef() {
                    ElementType = "text",
                    Text = "New function (",
                    RegionType = "args",
                    MultiRegion = true,
                },
                new SEElementDef() {
                    ElementType = "text",
                    Text = ")",
                    IndentMod = 1,
                    RegionType = "block",
                    MultiRegion = true,
                },
                new SEElementDef() {
                    ElementType = "text",
                    Text = "End",
                    IndentMod = -1,
                    RegionType = "end",
                },
            },
            CompileFunc = delegate (string[] regions) {
                return "function(" + regions[0] + "){" + regions[1] + "}";
            },
        },
        new SEBlockDef(){
            Name = "if",
            CursorIndex = 0,
            Flags = SEBlockDef.F_CONTROL_FLOW,
            Type = "control flow",
            Elements = new SEElementDef[]{
                new SEElementDef() {
                    ElementType = "text",
                    Text = "If (",
                    RegionType = "condition",
                },
                new SEElementDef() {
                    ElementType = "text",
                    Text = ")",
                    IndentMod = 1,
                    RegionType = "block",
                    MultiRegion = true,
                },
                new SEElementDef() {
                    ElementType = "text",
                    Text = "End",
                    IndentMod = -1,
                    RegionType = "end",
                },
            },
            CompileFunc = delegate (string[] regions) {
                return "if(" + regions[0] + "){" + regions[1] + "}";
            },
        },
        new SEBlockDef(){
            Name = "api_move_to",
            CursorIndex = 0,
            Flags = SEBlockDef.F_HAS_PROCEDURE,
            Type = "command",
            Elements = new SEElementDef[]{
                new SEElementDef() {
                    ElementType = "text",
                    Text = "Move to x=(",
                    RegionType = "expr",
                },
                new SEElementDef() {
                    ElementType = "text",
                    Text = ") y=(",
                    RegionType = "expr",
                },
                new SEElementDef() {
                    ElementType = "text",
                    Text = ")",
                    RegionType = "end",
                },
            },
            CompileFunc = delegate (string[] regions) {
                return "if(" + regions[0] + "){" + regions[1] + "}";
            },
        },
    };
        
    private BasicAPI() {
        
    }
    
    private void Print(string str) {
        Debug.Log(str);
    }
    
    public void Register(ScriptSystem scriptSystem) {
        scriptSystem.RegisterFunction("print", new Action<string>(Print), false);
        
        scriptSystem.RegisterJSFunction(
            "_closer_to", 
            new string[]{"i", "from", "to"}, 
            @"
                if (i > to) --i;
                else if (i < to) ++i;
                else {
                    if (to > from) ++i;
                    else --i;
                }
                return i;
            ",
            false
        );
        
        scriptSystem.RegisterJSFunction(
            "_in_between", 
            new string[]{"i", "a", "b"}, 
            @"
                if (a < b) {
                    return a <= i && i <= b;
                }
                else {
                    return b <= i && i <= a;
                }
            ",
            false
        );
        
        /* scriptSystem.RegisterMacro(
            @"\brepeat\s*" + ScriptSystem.MatchingBracketPattern("param"),
            @"for (var _ = 0; _ < ${param}; ++_)"
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
        
        scriptSystem.RegisterEvent("start", new string[]{});
        
        foreach (var blockDef in blockDefs) {
            scriptSystem.RegisterBlockDef(blockDef);
        }
    }

    public void PreRun(ScriptSystem scriptSystem) {
        
    }
    
    public void PostRun(ScriptSystem scriptSystem) {
        
    }
}
