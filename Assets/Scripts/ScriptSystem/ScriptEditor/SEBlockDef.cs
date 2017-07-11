using System;
using System.Collections.Generic;

public class SEBlockDef {
    
    public delegate string CompileFuncDelegate(string[] regions);
    
    public SEElementDef[] Elements = null;
    public string Name = null;
    public int CursorIndex = 0;
    public int Flags = 0;
    public string Type = null;
    public CompileFuncDelegate CompileFunc = null;
    
    private SEElementDef apiElementDef = null;
    
    public SEBlockDef() {
        
    }
    
    public const int F_RETURN_VAL = 1 << 0;
    public const int F_CONTROL_FLOW = 1 << 1;
    public const int F_HAS_PROCEDURE = 1 << 2;
    public const int F_RETURN_BOOL = 1 << 3;
    public const int F_DEFINITION = 1 << 4;
    public const int F_ARG = 1 << 5;
    
    private static Dictionary<string, int> regionFlags = new Dictionary<string, int>() {
        {"none", 0},
        {"top", F_DEFINITION},
        {"end", -1},
        {"expr", F_RETURN_VAL},
        {"block", F_CONTROL_FLOW | F_HAS_PROCEDURE},
        {"condition", F_RETURN_BOOL},
        {"args", F_ARG},
    };
    
    private static Dictionary<string, float[]> typeColors = new Dictionary<string, float[]>() {
        {"command", new float[]{1.0f, 0.0f, 1.0f, 1.0f}},
        {"definition", new float[]{0.1f, 0.8f, 0.1f, 1.0f}},
        {"control flow", new float[]{0.1f, 0.3f, 1.0f, 1.0f}},
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
            this.apiElementDef.Text = this.Name;
        }
        
        return this.apiElementDef;
    }
    
}
