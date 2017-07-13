using System;
using System.Collections.Generic;

public class SEBlockDef {
    
    public delegate string CompileFuncDelegate(string[] regions, string[] inputs);
    
    public SEElementDef[] Elements         = null;
    public string Name                     = null;
    public int CursorIndex                 = 0;
    public int Flags                       = 0;
    public string Type                     = null;
    public CompileFuncDelegate CompileFunc = null;
    
    private string displayName = null;
    public string DisplayName {
        get {
            if (this.displayName == null) {
                this.displayName = this.GenerateDisplayName();
            }
            return this.displayName;
        }
        set {
            this.displayName = value;
        }
    }
    
    private SEElementDef apiElementDef = null;
    
    public SEBlockDef() {
        
    }
    
    public const int F_RETURN_VAL = 1 << 0;
    public const int F_CONTROL_FLOW = 1 << 1;
    public const int F_HAS_PROCEDURE = 1 << 2;
    public const int F_RETURN_BOOL = 1 << 3;
    public const int F_DEFINITION = 1 << 4;
    public const int F_ARG = 1 << 5;
    public const int F_COMMENT = 1 << 6;
    public const int F_ALL = ~0;
    
    private static Dictionary<string, int> regionFlags = new Dictionary<string, int>() {
        {"none", 0},
        {"top", F_DEFINITION | F_COMMENT},
        {"end", -1},
        {"expr", F_RETURN_VAL},
        {"block", F_CONTROL_FLOW | F_HAS_PROCEDURE | F_COMMENT},
        {"condition", F_RETURN_BOOL},
        {"args", F_ARG},
    };
    
    private static Dictionary<string, float[]> typeColors = new Dictionary<string, float[]>() {
        {"js", new float[]{0.7f, 0.0f, 0.0f, 1.0f}},
        {"others", new float[]{0.4f, 0.4f, 0.4f, 1.0f}},
        {"value", new float[]{0.1f, 0.3f, 1.0f, 1.0f}},
        {"command", new float[]{1.0f, 0.0f, 1.0f, 1.0f}},
        {"comparison", new float[]{0.6f, 0.0f, 0.9f, 1.0f}},
        {"operator", new float[]{0.5f, 0.7f, 0.2f, 1.0f}},
        {"event", new float[]{0.0f, 0.5f, 0.5f, 1.0f}},
        {"function", new float[]{0.1f, 0.8f, 0.1f, 1.0f}},
        {"control flow", new float[]{0.8f, 0.3f, 0.1f, 1.0f}},
    };
    
    public static int GetRegionFlag(string regionName) {
        return regionFlags[regionName];
    }
    
    public static float[] GetTypeColor(string type) {
        return typeColors[type];
    }
    
    public static bool CompareFlags(int flags1, int flags2) {
        return (flags1 & flags2) != 0;
    }
    
    public bool CompareFlags(int flags) {
        return CompareFlags(this.Flags, flags);
    }
    
    public SEElementDef GenerateAPIElementDef() {
        if (this.apiElementDef == null) {
            this.apiElementDef = new SEElementDef();
            this.apiElementDef.BlockDefName = this.Name;
            this.apiElementDef.Color = GetTypeColor(this.Type);
            this.apiElementDef.ElementType = "api";
            this.apiElementDef.Text = this.DisplayName;
        }
        
        return this.apiElementDef;
    }
    
    public string GenerateDisplayName() {
        string result = "";
        
        foreach (var elementDef in this.Elements) {
            if (elementDef.ElementType == "input") {
                result += "[]";
            }
            else if (elementDef.ElementType == "text") {
                result += elementDef.Text;
            }
            
            if (elementDef.RegionType == "expr") {
                result += "..";
            }
            if (elementDef.RegionType == "condition") {
                result += "?";
            }
            if (elementDef.RegionType == "block") {
                result += " ... ";
            }
        }
        
        result = result.Replace('_', ' ');
        
        return result;
    }
    
}
