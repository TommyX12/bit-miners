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
            DisplayName = "raw javascript",
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
            DisplayName = "new function",
            Name = "function",
            CursorIndex = 1,
            Flags = SEBlockDef.F_DEFINITION,
            Type = "function",
            Elements = new SEElementDef[]{
                new SEElementDef() {
                    ElementType = "text",
                    Text = "new function",
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
                    Text = "end",
                    IndentMod = -1,
                    RegionType = "end",
                },
            },
            CompileFunc = delegate (string[] regions, string[] inputs) {
                return "function " + inputs[0] + "(" + regions[2] + "){" + regions[3] + "}";
            },
        },
        new SEBlockDef(){
            DisplayName = "add parameter",
            Name = "arg",
            CursorIndex = 1,
            Flags = SEBlockDef.F_ARG,
            Type = "function",
            Elements = new SEElementDef[]{
                new SEElementDef() {
                    ElementType = "text",
                    Text = "param",
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
            DisplayName = "run function",
            Name = "run",
            CursorIndex = 1,
            Flags = SEBlockDef.F_HAS_PROCEDURE | SEBlockDef.F_RETURN_VAL,
            Type = "function",
            Elements = new SEElementDef[]{
                new SEElementDef() {
                    ElementType = "text",
                    Text = "run",
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
            DisplayName = "return",
            Name = "return",
            CursorIndex = 0,
            Flags = SEBlockDef.F_HAS_PROCEDURE,
            Type = "function",
            Elements = new SEElementDef[]{
                new SEElementDef() {
                    ElementType = "text",
                    Text = "return (",
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
            DisplayName = "equals to",
            Name = "==",
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
            DisplayName = "not equals to",
            Name = "!=",
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
                    Text = " not = ",
                    RegionType = "expr",
                },
                new SEElementDef() {
                    ElementType = "text",
                    Text = ")",
                    RegionType = "end",
                },
            },
            CompileFunc = delegate (string[] regions, string[] inputs) {
                return "(" + regions[0] + ")!==(" + regions[1] + ")";
            },
        },
        new SEBlockDef(){
            DisplayName = "smaller than",
            Name = "<",
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
            DisplayName = "larger than",
            Name = ">",
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
                    Text = " > ",
                    RegionType = "expr",
                },
                new SEElementDef() {
                    ElementType = "text",
                    Text = ")",
                    RegionType = "end",
                },
            },
            CompileFunc = delegate (string[] regions, string[] inputs) {
                return "(" + regions[0] + ")>(" + regions[1] + ")";
            },
        },
        new SEBlockDef(){
            DisplayName = "smaller than or equals",
            Name = "<=",
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
                    Text = " <= ",
                    RegionType = "expr",
                },
                new SEElementDef() {
                    ElementType = "text",
                    Text = ")",
                    RegionType = "end",
                },
            },
            CompileFunc = delegate (string[] regions, string[] inputs) {
                return "(" + regions[0] + ")<=(" + regions[1] + ")";
            },
        },
        new SEBlockDef(){
            DisplayName = "larger than or equals",
            Name = ">=",
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
                    Text = " >= ",
                    RegionType = "expr",
                },
                new SEElementDef() {
                    ElementType = "text",
                    Text = ")",
                    RegionType = "end",
                },
            },
            CompileFunc = delegate (string[] regions, string[] inputs) {
                return "(" + regions[0] + ")>=(" + regions[1] + ")";
            },
        },
        new SEBlockDef(){
            DisplayName = ".. and ..",
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
            DisplayName = ".. or ..",
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
            DisplayName = "not ..",
            Name = "not",
            CursorIndex = 0,
            Flags = SEBlockDef.F_RETURN_BOOL,
            Type = "comparison",
            Elements = new SEElementDef[]{
                new SEElementDef() {
                    ElementType = "text",
                    Text = "not (",
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
            DisplayName = ".. + ..",
            Name = "+",
            CursorIndex = 0,
            Flags = SEBlockDef.F_RETURN_VAL,
            Type = "operator",
            Elements = new SEElementDef[]{
                new SEElementDef() {
                    ElementType = "text",
                    Text = "(",
                    RegionType = "expr",
                },
                new SEElementDef() {
                    ElementType = "text",
                    Text = "+",
                    RegionType = "expr",
                },
                new SEElementDef() {
                    ElementType = "text",
                    Text = ")",
                    RegionType = "end",
                },
            },
            CompileFunc = delegate (string[] regions, string[] inputs) {
                return "(" + regions[0] + ")+(" + regions[1] + ")";
            },
        },
        new SEBlockDef(){
            DisplayName = ".. - ..",
            Name = "-",
            CursorIndex = 0,
            Flags = SEBlockDef.F_RETURN_VAL,
            Type = "operator",
            Elements = new SEElementDef[]{
                new SEElementDef() {
                    ElementType = "text",
                    Text = "(",
                    RegionType = "expr",
                },
                new SEElementDef() {
                    ElementType = "text",
                    Text = "-",
                    RegionType = "expr",
                },
                new SEElementDef() {
                    ElementType = "text",
                    Text = ")",
                    RegionType = "end",
                },
            },
            CompileFunc = delegate (string[] regions, string[] inputs) {
                return "(" + regions[0] + ")-(" + regions[1] + ")";
            },
        },
        new SEBlockDef(){
            DisplayName = ".. x ..",
            Name = "*",
            CursorIndex = 0,
            Flags = SEBlockDef.F_RETURN_VAL,
            Type = "operator",
            Elements = new SEElementDef[]{
                new SEElementDef() {
                    ElementType = "text",
                    Text = "(",
                    RegionType = "expr",
                },
                new SEElementDef() {
                    ElementType = "text",
                    Text = "x",
                    RegionType = "expr",
                },
                new SEElementDef() {
                    ElementType = "text",
                    Text = ")",
                    RegionType = "end",
                },
            },
            CompileFunc = delegate (string[] regions, string[] inputs) {
                return "(" + regions[0] + ")*(" + regions[1] + ")";
            },
        },
        new SEBlockDef(){
            DisplayName = ".. / ..",
            Name = "/",
            CursorIndex = 0,
            Flags = SEBlockDef.F_RETURN_VAL,
            Type = "operator",
            Elements = new SEElementDef[]{
                new SEElementDef() {
                    ElementType = "text",
                    Text = "(",
                    RegionType = "expr",
                },
                new SEElementDef() {
                    ElementType = "text",
                    Text = "/",
                    RegionType = "expr",
                },
                new SEElementDef() {
                    ElementType = "text",
                    Text = ")",
                    RegionType = "end",
                },
            },
            CompileFunc = delegate (string[] regions, string[] inputs) {
                return "(" + regions[0] + ")/(" + regions[1] + ")";
            },
        },
        new SEBlockDef(){
            DisplayName = "if .. then ...",
            Name = "if",
            CursorIndex = 0,
            Flags = SEBlockDef.F_CONTROL_FLOW,
            Type = "control flow",
            Elements = new SEElementDef[]{
                new SEElementDef() {
                    ElementType = "text",
                    Text = "if (",
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
                    Text = "end",
                    IndentMod = -1,
                    RegionType = "end",
                },
            },
            CompileFunc = delegate (string[] regions, string[] inputs) {
                return "if(" + regions[0] + "){" + regions[1] + "}";
            },
        },
        new SEBlockDef(){
            DisplayName = "... or if .. then ...",
            Name = "else if",
            CursorIndex = 0,
            Flags = SEBlockDef.F_CONTROL_FLOW,
            Type = "control flow",
            Elements = new SEElementDef[]{
                new SEElementDef() {
                    ElementType = "text",
                    Text = "... or if (",
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
                    Text = "end",
                    IndentMod = -1,
                    RegionType = "end",
                },
            },
            CompileFunc = delegate (string[] regions, string[] inputs) {
                return "else if(" + regions[0] + "){" + regions[1] + "}";
            },
        },
        new SEBlockDef(){
            DisplayName = "... otherwise ...",
            Name = "else",
            CursorIndex = 0,
            Flags = SEBlockDef.F_CONTROL_FLOW,
            Type = "control flow",
            Elements = new SEElementDef[]{
                new SEElementDef() {
                    ElementType = "text",
                    Text = "... otherwise",
                    IndentMod = 1,
                    RegionType = "block",
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
                return "else{" + regions[0] + "}";
            },
        },
        new SEBlockDef(){
            DisplayName = "set variable",
            Name = "set",
            CursorIndex = 2,
            Flags = SEBlockDef.F_HAS_PROCEDURE | SEBlockDef.F_DEFINITION,
            Type = "value",
            Elements = new SEElementDef[]{
                new SEElementDef() {
                    ElementType = "text",
                    Text = "set",
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
                    Text = "to (",
                    RegionType = "expr",
                },
                new SEElementDef() {
                    ElementType = "text",
                    Text = ")",
                    RegionType = "end",
                },
            },
            CompileFunc = delegate (string[] regions, string[] inputs) {
                return inputs[0] + "=" + regions[2];
            },
        },
        new SEBlockDef(){
            DisplayName = "increase by",
            Name = "+=",
            CursorIndex = 2,
            Flags = SEBlockDef.F_HAS_PROCEDURE,
            Type = "value",
            Elements = new SEElementDef[]{
                new SEElementDef() {
                    ElementType = "text",
                    Text = "increase",
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
                    Text = "by (",
                    RegionType = "expr",
                },
                new SEElementDef() {
                    ElementType = "text",
                    Text = ")",
                    RegionType = "end",
                },
            },
            CompileFunc = delegate (string[] regions, string[] inputs) {
                return inputs[0] + "+=" + regions[2];
            },
        },
        new SEBlockDef(){
            DisplayName = "decrease by",
            Name = "-=",
            CursorIndex = 2,
            Flags = SEBlockDef.F_HAS_PROCEDURE,
            Type = "value",
            Elements = new SEElementDef[]{
                new SEElementDef() {
                    ElementType = "text",
                    Text = "decrease",
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
                    Text = "by (",
                    RegionType = "expr",
                },
                new SEElementDef() {
                    ElementType = "text",
                    Text = ")",
                    RegionType = "end",
                },
            },
            CompileFunc = delegate (string[] regions, string[] inputs) {
                return inputs[0] + "-=" + regions[2];
            },
        },
        new SEBlockDef(){
            DisplayName = "value of variable",
            Name = "var",
            CursorIndex = 1,
            Flags = SEBlockDef.F_RETURN_VAL,
            Type = "value",
            Elements = new SEElementDef[]{
                new SEElementDef() {
                    ElementType = "text",
                    Text = "var",
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
            DisplayName = "text",
            Name = "text",
            CursorIndex = 1,
            Flags = SEBlockDef.F_RETURN_VAL,
            Type = "value",
            Elements = new SEElementDef[]{
                new SEElementDef() {
                    ElementType = "text",
                    Text = "text",
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
            DisplayName = "number",
            Name = "num",
            CursorIndex = 1,
            Flags = SEBlockDef.F_RETURN_VAL,
            Type = "value",
            Elements = new SEElementDef[]{
                new SEElementDef() {
                    ElementType = "text",
                    Text = "num",
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
            DisplayName = "true",
            Name = "true",
            CursorIndex = 0,
            Flags = SEBlockDef.F_RETURN_VAL | SEBlockDef.F_RETURN_BOOL,
            Type = "value",
            Elements = new SEElementDef[]{
                new SEElementDef() {
                    ElementType = "text",
                    Text = "true",
                    RegionType = "end",
                },
            },
            CompileFunc = delegate (string[] regions, string[] inputs) {
                return "true";
            },
        },
        new SEBlockDef(){
            DisplayName = "false",
            Name = "false",
            CursorIndex = 0,
            Flags = SEBlockDef.F_RETURN_VAL | SEBlockDef.F_RETURN_BOOL,
            Type = "value",
            Elements = new SEElementDef[]{
                new SEElementDef() {
                    ElementType = "text",
                    Text = "false",
                    RegionType = "end",
                },
            },
            CompileFunc = delegate (string[] regions, string[] inputs) {
                return "false";
            },
        },
        new SEBlockDef(){
            DisplayName = "nothing",
            Name = "undefined",
            CursorIndex = 0,
            Flags = SEBlockDef.F_RETURN_VAL,
            Type = "value",
            Elements = new SEElementDef[]{
                new SEElementDef() {
                    ElementType = "text",
                    Text = "nothing",
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
