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
    
    // public const string EVENT_HANDLER_PREFIX = "_on_";
    
    private List<IScriptSystemAPI> apiList;
    private List<Function> functions;
    private List<Macro> macros;
    private List<JavaScript> javaScripts;
    private List<Event> events;
    private Dictionary<string, SEBlockDef> blockDefs;
    
    public bool Running {
        get; private set;
    }
    
    public bool ErrorCaught {
        get; private set;
    }
    
    public int ErrorLine {
        get; private set;
    }
    
    public string Script {
        get; set;
    }
    
    public string BlockScript {
        get; set;
    }
    
    public string Message {
        get; private set;
    }
    
    public ScriptSystem(List<IScriptSystemAPI> apiList = null) {
        if (apiList == null) apiList = new List<IScriptSystemAPI>();
        this.SetAPIList(apiList);
        this.Running = false;
        this.ErrorCaught = false;
        this.Script = "";
        this.BlockScript = "[]";
        this.Message = "Idle";
        
        this.timeoutHelper = new ScriptTimeoutHelper();
    }
    
    public void SetAPIList(List<IScriptSystemAPI> apiList) {
        this.apiList = new List<IScriptSystemAPI>(apiList);
        this.apiList.Insert(0, BasicAPI.GetInstance());
        
        this.macros = new List<Macro>();
        this.functions = new List<Function>();
        this.javaScripts = new List<JavaScript>();
        this.events = new List<Event>();
        this.blockDefs = new Dictionary<string, SEBlockDef>();
        
        foreach (IScriptSystemAPI api in this.apiList) {
            api.Register(this);
        }
    }
    
    public void RegisterFunction(string functionName, Delegate functionDelegate, bool listed = true) {
        Function function = new Function(functionName, functionDelegate, listed);
        this.functions.Add(function);
        if (listed) {
            this.RegisterBlockDef(function.GenerateBlockDef());
        }
    }
    
    public void RegisterJSFunction(string functionName, string[] functionParams, string body, bool listed = true) {
        body = GetFunctionJS(functionName, functionParams, body);
        JavaScript js = new JavaScript(body, listed, functionName, functionParams);
        this.javaScripts.Add(js);
        if (listed) {
            this.RegisterBlockDef(js.GenerateBlockDef());
        }
    }
    
    public void RegisterJavaScript(string script) {
        this.javaScripts.Add(new JavaScript(script, false));
    }
    
    public void RegisterMacro(string pattern, string replacement, bool listed = true) {
        this.macros.Add(new Macro(pattern, replacement, listed));
    }
    
    public void RegisterEvent(string eventName, string[] eventParameters, bool listed = true) {
        Event event_ = new Event(eventName, eventParameters, listed);
        this.events.Add(event_);
        if (listed) {
            this.RegisterBlockDef(event_.GenerateBlockDef());
        }
    }
    
    public void RegisterBlockDef(SEBlockDef blockDef) {
        this.blockDefs[blockDef.Name] = blockDef;
    }
    
    public Dictionary<string, SEBlockDef> GetBlockDefs() {
        return this.blockDefs;
    }
    
    private void ApplyFunction(Function function) {
        if (function.DelegateObject == null) return;
        
        string wrapperArguments = "";
        int numArguments = function.DelegateObject.Method.GetParameters().Length;
        for (int i = 0; i < numArguments; ++i) {
            if (i > 0) wrapperArguments += ", ";
            wrapperArguments += "a" + i;
        }
        string wrapperFunction = @"
            function " + function.Name + "(" + wrapperArguments + @") {
                _critical_on();
                var result = __" + function.Name +"__(" + wrapperArguments + @");
                _critical_off();
                
                return result;
            }
        ";
        this.engine.SetGlobalFunction("__" + function.Name + "__", function.DelegateObject);
        this.engine.Execute(wrapperFunction);
    }
    
    private void ApplyVariable(string variableName, object variableValue) {
        this.engine.SetGlobalValue(variableName, variableValue);
    }
    
    private void ApplyJavaScript(JavaScript javaScript) {
        this.engine.Execute(javaScript.Script);
    }
    
    public void ExecuteJavaScript(string script) {
        this.engine.Execute(script);
    }
    
    public T EvalJavaScript<T>(string script) {
        return this.engine.Evaluate<T>(script);
    }
    
    private void ApplyMacro(ref string script, Macro macro) {
        script = Regex.Replace(script, macro.Pattern, macro.Replacement, RegexOptions.IgnorePatternWhitespace);
    }
    
    public string GetAPIListText() {
        string result = "";
        
        result += "<b>-- Functions --</b>";
        foreach (Function function in this.functions) {
            if (!function.Listed) continue;
            result += "\n" + function.GetText();
        }
        foreach (JavaScript javaScript in this.javaScripts) {
            if (!javaScript.Listed) continue;
            result += "\n" + javaScript.GetText();
        }
        result += "\n\n";
        
        result += "<b>-- Events --</b>";
        foreach (Event eventObject in this.events) {
            if (!eventObject.Listed) continue;
            result += "\n" + eventObject.GetText();
        }
        
        return result;
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
        string compiledScript = this.Script;
        
        // API application
        this.engine.SetGlobalFunction("_critical_on", new Action(this.timeoutHelper.EnterCriticalSection));
        this.engine.SetGlobalFunction("_critical_off", new Action(this.timeoutHelper.ExitCriticalSection));
        foreach (Function function in this.functions) {
            this.ApplyFunction(function);
        }
        foreach (JavaScript javaScript in this.javaScripts) {
            this.ApplyJavaScript(javaScript);
        }
        foreach (Macro macro in this.macros) {
            this.ApplyMacro(ref compiledScript, macro);
        }
        foreach (IScriptSystemAPI api in this.apiList) {
            api.PreRun(this);
        }
        
        // Status update
        this.Running = true;
        this.ErrorCaught = false;
        this.Message = "Standby";
        
        // Execution
        this.ExecuteAction(() => {
            this.engine.Execute(compiledScript);
        });
        this.DispatchEvent("start");
        
        // API post application
        foreach (IScriptSystemAPI api in this.apiList) {
            api.PostRun(this);
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
        
        // string handlerName = EVENT_HANDLER_PREFIX + name;
        // if (this.engine.Evaluate<bool>("this[\'" + handlerName + "\'] === undefined")) return;
        
        object[] scriptArgs = new object[args.Length + 1];
        scriptArgs[0] = name;
        for (int i = 0; i < args.Length; ++i) {
            scriptArgs[i + 1] = args[i];
        }
            
        this.ExecuteAction(() => {
            this.engine.CallGlobalFunction("event_dispatch", scriptArgs);
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
                this.ErrorLine = -1;
            }
            else if (ex is OutOfMemoryException) {
                this.Message = "Out of memory";
                this.ErrorLine = -1;
            }
            else if (ex is StackOverflowException) {
                this.Message = "Recursion depth limit exceeded";
                this.ErrorLine = -1;
            }
            else if (ex is Jurassic.JavaScriptException) {
                Jurassic.JavaScriptException jsex = (Jurassic.JavaScriptException)ex;
                this.Message = "Line " + jsex.LineNumber + " - " + jsex.ErrorObject.ToString();
                this.ErrorLine = jsex.LineNumber;
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
    
    private static string GetFunctionJS(string name, string[] parameters, string body) {
        return "function " + name + "(" + string.Join(", ", parameters) + ") {" + body + "}";
    }
    
    private static string GetFunctionAPIText(string name, string[] parameters) {
        string parametersString = "";
        for (int i = 0; i < parameters.Length; ++i) {
            var parameter = parameters[i];
            if (i > 0) parametersString += ", ";
            parametersString += Util.ColoredRichText("#88FFAA", parameter);
        }
        return Util.ColoredRichText("#88AAFF", name) + "(" + parametersString + ")";
    }
    
    private static string[] DelegateToParameters(Delegate delegateObject) {
        var parameters = delegateObject.Method.GetParameters();
        string[] result = new string[parameters.Length];
        for (int i = 0; i < parameters.Length; ++i) {
            var parameter = parameters[i];
            result[i] = parameter.Name;
        }
        
        return result;
    }
    
    private interface APIElement {
        bool Listed {
            get; set;
        }
        
        string GetText();
    }
    
    private class Function : APIElement {
        public string Name;
        public Delegate DelegateObject;
        public string[] Parameters;
        public bool Listed {
            get; set;
        }
        
        public Function(string name, Delegate delegateObject, bool listed = true)
        {
            this.Name = name;
            this.DelegateObject = delegateObject;
            this.Parameters = null;
            if (this.DelegateObject != null) {
                this.Parameters = ScriptSystem.DelegateToParameters(this.DelegateObject);
            }
            this.Listed = listed;
        }
        
        public string GetText() {
            return ScriptSystem.GetFunctionAPIText(this.Name, this.Parameters);
        }
        
        public SEBlockDef GenerateBlockDef() {
            return FuncToBlockDef(this.Name, this.Parameters, this.DelegateObject.Method.ReturnType != typeof(void));
        }
        
    }
    
    private class Macro : APIElement {
        public string Pattern;
        public string Replacement;
        public bool Listed {
            get; set;
        }
        
        public Macro(string pattern, string replacement, bool listed = true)
        {
            this.Pattern = pattern;
            this.Replacement = replacement;
            this.Listed = listed;
        }
        
        public string GetText() {
            return "";
        }
    }
    
    private class JavaScript : APIElement {
        public string Script;
        public string Name;
        public string[] Parameters;
        public bool Listed {
            get; set;
        }
        
        public JavaScript(string script, bool listed = true, string name = "", string[] parameters = null)
        {
            this.Script = script;
            this.Name = name;
            this.Parameters = parameters == null ? new string[0] : (string[])parameters.Clone();
            this.Listed = listed;
        }
        
        public string GetText() {
            return ScriptSystem.GetFunctionAPIText(this.Name, this.Parameters);
        }
        
        public SEBlockDef GenerateBlockDef() {
            return FuncToBlockDef(this.Name, this.Parameters, true);
        }
        
    }
    
    private class Event : APIElement {
        public string Name;
        public string[] Parameters;
        public bool Listed {
            get; set;
        }
        
        public Event(string name, string[] parameters, bool listed = true)
        {
            this.Name = name;
            this.Parameters = (string[])parameters.Clone();
            this.Listed = listed;
        }
        
        public string GetText() {
            return ScriptSystem.GetFunctionAPIText(this.Name, this.Parameters);
        }
        
        public SEBlockDef GenerateBlockDef() {
            string displayName = this.Name.Replace('_', ' ');
            SEBlockDef blockDef = new SEBlockDef() {
                DisplayName = displayName,
                Name = "_event_" + this.Name,
                CursorIndex = 0,
                Flags = SEBlockDef.F_DEFINITION,
                Type = "event",
                // CompileFunc = new EventCompileFuncWrapper(this.Name, this.Parameters).CompileFunc,
                Elements = new SEElementDef[]{
                    new SEElementDef() {
                        ElementType = "text",
                        Text = "when " + displayName,
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
                    string result = "event_add_listener(\"" + this.Name + "\", function(";
                    int j = 0;
                    foreach (string param in this.Parameters) {
                        if (j > 0) {
                            result += ",";
                        }
                        result += param;
                        j++;
                    }
                    result += "){" + regions[0] + "})";
                    
                    return result;
                },
            };
            
            return blockDef;
        }
    }
    
    public static SEBlockDef FuncToBlockDef(string name, string[] parameters, bool hasReturnVal) {
        SEBlockDef blockDef = new SEBlockDef() {
            Name = "_api_" + name,
            CursorIndex = 0,
            Flags = SEBlockDef.F_HAS_PROCEDURE,
            Type = "command",
            CompileFunc = delegate (string[] regions, string[] inputs) {
                string result = name + "(";
                int j = 0;
                foreach (string region in regions) {
                    if (j > 0) {
                        result += ",";
                    }
                    result += region;
                    j++;
                }
                result += ")";
                
                return result;
            },
        };
        
        if (hasReturnVal) {
            blockDef.Flags |= SEBlockDef.F_RETURN_VAL;
        }
        
        SEElementDef[] elements = new SEElementDef[parameters.Length + 1];
        
        int i = 0;
        for (i = 0; i < elements.Length; ++i) {
            SEElementDef element = new SEElementDef() {
                ElementType = "text",
                RegionType = "expr",
                Text = "",
            };
            elements[i] = element;
        }
        elements[0].Text = name.Replace('_', ' ');
        elements[elements.Length - 1].RegionType = "end";
        if (parameters.Length > 0) {
            elements[0].Text += " (";
            elements[elements.Length - 1].Text = ")";
        }
        
        i = 0;
        foreach (string param in parameters) {
            elements[i].Text += param.Replace('_', ' ') + "=";
            i++;
        }
        
        blockDef.Elements = elements;
        
        return blockDef;
    }
}
