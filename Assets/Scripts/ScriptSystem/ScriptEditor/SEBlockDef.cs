using System;
using System.Collections.Generic;

public class SEBlockDef {
    
    public delegate string CompileFuncDelegate(string[] regions);
    
    public SEElementDef[] Elements = null;
    public string Name = null;
    public int CursorIndex = 0;
    public int Flags = 0;
    public CompileFuncDelegate CompileFunc = null;
    
    public SEBlockDef() {
        
    }
    
    public const int F_RETURN_VAL = 1 << 0;
    public const int F_CONTROL_FLOW = 1 << 1;
    public const int F_HAS_PROCEDURE = 1 << 2;
    public const int F_RETURN_BOOL = 1 << 3;
    
    private static Dictionary<string, SEBlockDef> presets = new Dictionary<string, SEBlockDef>() {
        {"if",
            new SEBlockDef(){
                CursorIndex = 0,
                Flags = F_CONTROL_FLOW,
                Elements = new SEElementDef[]{
                    new SEElementDef() {
                        ElementType = "text",
                        Color = new float[]{0, 0.2f, 1, 1},
                        Text = "If (",
                        RegionType = "condition",
                    },
                    new SEElementDef() {
                        ElementType = "text",
                        Color = new float[]{0, 0.2f, 1, 1},
                        Text = ")",
                        RegionType = "block",
                    },
                    new SEElementDef() {
                        ElementType = "text",
                        Color = new float[]{0, 0.2f, 1, 1},
                        Text = "End",
                        RegionType = "end",
                    },
                },
                CompileFunc = delegate (string[] regions) {
                    return "if(" + regions[0] + "){" + regions[1] + "}";
                },
            }
        },
        {"api_move_to",
            new SEBlockDef(){
                CursorIndex = 0,
                Flags = F_HAS_PROCEDURE,
                Elements = new SEElementDef[]{
                    new SEElementDef() {
                        ElementType = "text",
                        Color = new float[]{1, 0, 1, 1},
                        Text = "Move to x=(",
                        RegionType = "expr",
                    },
                    new SEElementDef() {
                        ElementType = "text",
                        Color = new float[]{1, 0, 1, 1},
                        Text = ") y=(",
                        RegionType = "expr",
                    },
                    new SEElementDef() {
                        ElementType = "text",
                        Color = new float[]{1, 0, 1, 1},
                        Text = ")",
                        RegionType = "end",
                    },
                },
                CompileFunc = delegate (string[] regions) {
                    return "if(" + regions[0] + "){" + regions[1] + "}";
                },
            }
        },
    };
    
    private static Dictionary<string, int> regionFlags = new Dictionary<string, int>() {
        {"end", -1},
        {"none", 0},
        {"expr", F_RETURN_VAL},
        {"block", F_CONTROL_FLOW | F_HAS_PROCEDURE},
        {"condition", F_RETURN_BOOL},
    };
    
    static SEBlockDef(){
        foreach (var entry in presets) {
            entry.Value.Name = entry.Key;
        }
    }
    
    public static SEBlockDef GetPreset(string name) {
        return presets[name];
    }
    
    public static int GetRegionFlag(string regionName) {
        return regionFlags[regionName];
    }
    
}
