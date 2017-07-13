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
            Name = "js",
            CursorIndex = 1,
            Flags = SEBlockDef.F_JS,
            Type = "js",
            Elements = new SEElementDef[]{
                new SEElementDef() {
                    ElementType = "text",
                    Text = "JS",
                    ExtendSize = true,
                    RegionType = "none",
                },
                new SEElementDef() {
                    ElementType = "input",
                    Text = "",
                    InputType = "js",
                    RegionType = "end",
                },
            },
            CompileFunc = delegate (string[] regions, string[] inputs) {
                return inputs[0];
            },
        },
        new SEBlockDef(){
            Name = "function",
            CursorIndex = 1,
            Flags = SEBlockDef.F_DEFINITION,
            Type = "function",
            Elements = new SEElementDef[]{
                new SEElementDef() {
                    ElementType = "text",
                    Text = "New function",
                    ExtendSize = true,
                    RegionType = "none",
                },
                new SEElementDef() {
                    ElementType = "input",
                    Text = "",
                    InputType = "id",
                    RegionType = "end",
                },
                new SEElementDef() {
                    ElementType = "text",
                    Text = "(",
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
            CompileFunc = delegate (string[] regions, string[] inputs) {
                return "function " + inputs[0] + "(" + regions[2] + "){" + regions[3] + "}";
            },
        },
        new SEBlockDef(){
            Name = "arg",
            CursorIndex = 1,
            Flags = SEBlockDef.F_ARG,
            Type = "function",
            Elements = new SEElementDef[]{
                new SEElementDef() {
                    ElementType = "text",
                    Text = "Param",
                    ExtendSize = true,
                    RegionType = "none",
                },
                new SEElementDef() {
                    ElementType = "input",
                    Text = "",
                    InputType = "id",
                    RegionType = "end",
                },
            },
            CompileFunc = delegate (string[] regions, string[] inputs) {
                return inputs[0];
            },
        },
        new SEBlockDef(){
            Name = "run",
            CursorIndex = 1,
            Flags = SEBlockDef.F_HAS_PROCEDURE | SEBlockDef.F_RETURN_VAL,
            Type = "function",
            Elements = new SEElementDef[]{
                new SEElementDef() {
                    ElementType = "text",
                    Text = "Run",
                    ExtendSize = true,
                    RegionType = "none",
                },
                new SEElementDef() {
                    ElementType = "input",
                    Text = "",
                    InputType = "id",
                    RegionType = "none",
                },
                new SEElementDef() {
                    ElementType = "text",
                    Text = "(",
                    RegionType = "expr",
                    MultiRegion = true,
                },
                new SEElementDef() {
                    ElementType = "text",
                    Text = ")",
                    RegionType = "end",
                },
            },
            CompileFunc = delegate (string[] regions, string[] inputs) {
                return inputs[0] + "(" + regions[2] + ")";
            },
        },
        new SEBlockDef(){
            Name = "return",
            CursorIndex = 0,
            Flags = SEBlockDef.F_HAS_PROCEDURE,
            Type = "function",
            Elements = new SEElementDef[]{
                new SEElementDef() {
                    ElementType = "text",
                    Text = "Return (",
                    RegionType = "expr",
                },
                new SEElementDef() {
                    ElementType = "text",
                    Text = ")",
                    RegionType = "end",
                },
            },
            CompileFunc = delegate (string[] regions, string[] inputs) {
                return "return " + regions[0];
            },
        },
        new SEBlockDef(){
            Name = "equals_to",
            CursorIndex = 0,
            Flags = SEBlockDef.F_RETURN_BOOL,
            Type = "comparison",
            Elements = new SEElementDef[]{
                new SEElementDef() {
                    ElementType = "text",
                    Text = "(",
                    RegionType = "expr",
                },
                new SEElementDef() {
                    ElementType = "text",
                    Text = " = ",
                    RegionType = "expr",
                },
                new SEElementDef() {
                    ElementType = "text",
                    Text = ")",
                    RegionType = "end",
                },
            },
            CompileFunc = delegate (string[] regions, string[] inputs) {
                return "(" + regions[0] + ")===(" + regions[1] + ")";
            },
        },
        new SEBlockDef(){
            Name = "smaller_than",
            CursorIndex = 0,
            Flags = SEBlockDef.F_RETURN_BOOL,
            Type = "comparison",
            Elements = new SEElementDef[]{
                new SEElementDef() {
                    ElementType = "text",
                    Text = "(",
                    RegionType = "expr",
                },
                new SEElementDef() {
                    ElementType = "text",
                    Text = " < ",
                    RegionType = "expr",
                },
                new SEElementDef() {
                    ElementType = "text",
                    Text = ")",
                    RegionType = "end",
                },
            },
            CompileFunc = delegate (string[] regions, string[] inputs) {
                return "(" + regions[0] + ")<(" + regions[1] + ")";
            },
        },
        new SEBlockDef(){
            Name = "and",
            CursorIndex = 0,
            Flags = SEBlockDef.F_RETURN_BOOL,
            Type = "comparison",
            Elements = new SEElementDef[]{
                new SEElementDef() {
                    ElementType = "text",
                    Text = "(",
                    RegionType = "condition",
                },
                new SEElementDef() {
                    ElementType = "text",
                    Text = "and",
                    RegionType = "condition",
                },
                new SEElementDef() {
                    ElementType = "text",
                    Text = ")",
                    RegionType = "end",
                },
            },
            CompileFunc = delegate (string[] regions, string[] inputs) {
                return "(" + regions[0] + ")&&(" + regions[1] + ")";
            },
        },
        new SEBlockDef(){
            Name = "or",
            CursorIndex = 0,
            Flags = SEBlockDef.F_RETURN_BOOL,
            Type = "comparison",
            Elements = new SEElementDef[]{
                new SEElementDef() {
                    ElementType = "text",
                    Text = "(",
                    RegionType = "condition",
                },
                new SEElementDef() {
                    ElementType = "text",
                    Text = "or",
                    RegionType = "condition",
                },
                new SEElementDef() {
                    ElementType = "text",
                    Text = ")",
                    RegionType = "end",
                },
            },
            CompileFunc = delegate (string[] regions, string[] inputs) {
                return "(" + regions[0] + ")||(" + regions[1] + ")";
            },
        },
        new SEBlockDef(){
            Name = "not",
            CursorIndex = 0,
            Flags = SEBlockDef.F_RETURN_BOOL,
            Type = "comparison",
            Elements = new SEElementDef[]{
                new SEElementDef() {
                    ElementType = "text",
                    Text = "Not (",
                    RegionType = "condition",
                },
                new SEElementDef() {
                    ElementType = "text",
                    Text = ")",
                    RegionType = "end",
                },
            },
            CompileFunc = delegate (string[] regions, string[] inputs) {
                return "!(" + regions[0] + ")";
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
            CompileFunc = delegate (string[] regions, string[] inputs) {
                return "if(" + regions[0] + "){" + regions[1] + "}";
            },
        },
        new SEBlockDef(){
            Name = "else if",
            CursorIndex = 0,
            Flags = SEBlockDef.F_CONTROL_FLOW,
            Type = "control flow",
            Elements = new SEElementDef[]{
                new SEElementDef() {
                    ElementType = "text",
                    Text = "Otherwise if (",
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
            CompileFunc = delegate (string[] regions, string[] inputs) {
                return "else if(" + regions[0] + "){" + regions[1] + "}";
            },
        },
        new SEBlockDef(){
            Name = "else",
            CursorIndex = 0,
            Flags = SEBlockDef.F_CONTROL_FLOW,
            Type = "control flow",
            Elements = new SEElementDef[]{
                new SEElementDef() {
                    ElementType = "text",
                    Text = "Otherwise",
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
            CompileFunc = delegate (string[] regions, string[] inputs) {
                return "else{" + regions[0] + "}";
            },
        },
        new SEBlockDef(){
            Name = "event_start",
            CursorIndex = 0,
            Flags = SEBlockDef.F_DEFINITION,
            Type = "event",
            Elements = new SEElementDef[]{
                new SEElementDef() {
                    ElementType = "text",
                    Text = "When start",
                    RegionType = "block",
                    IndentMod = 1,
                    MultiRegion = true,
                },
                new SEElementDef() {
                    ElementType = "text",
                    Text = "end",
                    IndentMod = -1,
                    RegionType = "end",
                },
            },
            CompileFunc = delegate (string[] regions, string[] inputs) {
                return "when start(){" + regions[0] + "}";
            },
        },
        new SEBlockDef(){
            Name = "api_move_in_direction",
            CursorIndex = 0,
            Flags = SEBlockDef.F_HAS_PROCEDURE,
            Type = "command",
            Elements = new SEElementDef[]{
                new SEElementDef() {
                    ElementType = "text",
                    Text = "Move in dir x=(",
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
            CompileFunc = delegate (string[] regions, string[] inputs) {
                return "move_in_direction(" + regions[0] + "," + regions[1] + ")";
            },
        },
        new SEBlockDef(){
            Name = "var",
            CursorIndex = 1,
            Flags = SEBlockDef.F_RETURN_VAL,
            Type = "value",
            Elements = new SEElementDef[]{
                new SEElementDef() {
                    ElementType = "text",
                    Text = "Var",
                    ExtendSize = true,
                    RegionType = "none",
                },
                new SEElementDef() {
                    ElementType = "input",
                    Text = "",
                    InputType = "id",
                    RegionType = "end",
                },
            },
            CompileFunc = delegate (string[] regions, string[] inputs) {
                return inputs[0];
            },
        },
        new SEBlockDef(){
            Name = "text",
            CursorIndex = 1,
            Flags = SEBlockDef.F_RETURN_VAL,
            Type = "value",
            Elements = new SEElementDef[]{
                new SEElementDef() {
                    ElementType = "text",
                    Text = "Text",
                    ExtendSize = true,
                    RegionType = "none",
                },
                new SEElementDef() {
                    ElementType = "input",
                    Text = "",
                    InputType = "str",
                    RegionType = "end",
                },
            },
            CompileFunc = delegate (string[] regions, string[] inputs) {
                return "\"" + Util.EscapeScriptString(inputs[0]) + "\"";
            },
        },
        new SEBlockDef(){
            Name = "num",
            CursorIndex = 1,
            Flags = SEBlockDef.F_RETURN_VAL,
            Type = "value",
            Elements = new SEElementDef[]{
                new SEElementDef() {
                    ElementType = "text",
                    Text = "Num",
                    ExtendSize = true,
                    RegionType = "none",
                },
                new SEElementDef() {
                    ElementType = "input",
                    Text = "",
                    InputType = "num",
                    RegionType = "end",
                },
            },
            CompileFunc = delegate (string[] regions, string[] inputs) {
                return inputs[0];
            },
        },
        new SEBlockDef(){
            Name = "true",
            CursorIndex = 0,
            Flags = SEBlockDef.F_RETURN_VAL | SEBlockDef.F_RETURN_BOOL,
            Type = "value",
            Elements = new SEElementDef[]{
                new SEElementDef() {
                    ElementType = "text",
                    Text = "True",
                    RegionType = "end",
                },
            },
            CompileFunc = delegate (string[] regions, string[] inputs) {
                return "true";
            },
        },
        new SEBlockDef(){
            Name = "false",
            CursorIndex = 0,
            Flags = SEBlockDef.F_RETURN_VAL | SEBlockDef.F_RETURN_BOOL,
            Type = "value",
            Elements = new SEElementDef[]{
                new SEElementDef() {
                    ElementType = "text",
                    Text = "False",
                    RegionType = "end",
                },
            },
            CompileFunc = delegate (string[] regions, string[] inputs) {
                return "false";
            },
        },
        new SEBlockDef(){
            Name = "undefined",
            CursorIndex = 0,
            Flags = SEBlockDef.F_RETURN_VAL,
            Type = "value",
            Elements = new SEElementDef[]{
                new SEElementDef() {
                    ElementType = "text",
                    Text = "Nothing",
                    RegionType = "end",
                },
            },
            CompileFunc = delegate (string[] regions, string[] inputs) {
                return "undefined";
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
