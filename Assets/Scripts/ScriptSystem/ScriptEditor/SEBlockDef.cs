using System;
using System.Collections.Generic;

public class SEBlockDef {
    
    public delegate string CompileFuncDelegate(string[] regions);
    
    public SEElementDef[] Elements = null;
    public string Name = null;
    public int CursorIndex = 0;
    public CompileFuncDelegate CompileFunc = null;
    
    public SEBlockDef() {
        
    }
    
    private static Dictionary<string, SEBlockDef> presets = new Dictionary<string, SEBlockDef>() {
        {"if",
            new SEBlockDef(){
                CursorIndex = 0,
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
    
    static SEBlockDef(){
        foreach (var entry in presets) {
            entry.Value.Name = entry.Key;
        }
    }
    
    public static SEBlockDef GetPreset(string name) {
        return presets[name];
    }
    
}
