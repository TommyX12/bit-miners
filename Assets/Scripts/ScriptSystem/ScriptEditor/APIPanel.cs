using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class APIPanel: SEElementContainer {
    
    public SEAPIElement SEAPIElementPrefab;
    public ScriptPanel ScriptPanelObject;
    
    private int cachedCursorFlags = -1;
    
    private Dictionary<string, SEBlockDef> blockDefs = null;
    
    private static readonly List<string> blockTypeOrder = new List<string>(){
        "event",
        "command",
        "calculation",
        "control flow",
        "comparison",
        "value",
        "function",
        "others",
        "js",
    };
    
    protected override void OnAwake() {
        this.VerticalSpacing = 1.0f;
    }
    
    protected override void OnStart() {
        this.ScrollToStart();
    }
    
    public override void NormalUpdate() {
        base.NormalUpdate();
        
        if (this.gameObject.activeInHierarchy) {
            // check if script panel cursor region flag has changed.
            int flags = this.ScriptPanelObject.GetCursorFlags();
            if (flags != this.cachedCursorFlags) {
                this.cachedCursorFlags = flags;
                this.RefreshFlags();
            }
        }
    }
    
    protected void RefreshFlags() {
        int lastLabel = 0;
        bool foundActive = false;
        for (int i = 0; i < this.data.Count; ++i) {
            List<SEElement> row = this.data[i];
            
            if (row.Count == 0) {
                this.data[lastLabel][0].SetActive(foundActive);
                foundActive = false;
                continue;
            }
            
            SEElementDef def = row[0].Definition;
            if (def.BlockDefName == null) {
                lastLabel = i;
                continue;
            }
            
            SEBlockDef blockDef = this.blockDefs[def.BlockDefName];
            bool active = blockDef.CompareFlags(this.cachedCursorFlags);
            row[0].SetActive(active);
            if (active) foundActive = true;
        }
    }
    
    protected override void OnRedraw() {
        this.RefreshFlags();
    }
    
    public void LoadBlockDefs(Dictionary<string, SEBlockDef> blockDefs) {
        this.ClearElements();
        this.blockDefs = blockDefs;
        
        Dictionary<string, List<SEBlockDef>> dict = new Dictionary<string, List<SEBlockDef>>();
        
        foreach (var blockDef in blockDefs.Values) {
            if (!dict.ContainsKey(blockDef.Type)) {
                dict[blockDef.Type] = new List<SEBlockDef>();
            }
            dict[blockDef.Type].Add(blockDef);
        }
        
        this.Redrawable = false;
        int row = 0;
        foreach (var type in blockTypeOrder) {
            this.InsertElement(row, 0, SEElementDef.GenerateAPILabel(type).SpawnElement(this.GetPrefab));
            row++;
            
            foreach (var blockDef in dict[type]) {
                this.InsertElement(row, 0, blockDef.GenerateAPIElementDef().SpawnElement(this.GetPrefab));
                row++;
            }
            
            this.InsertRow(row);
            row++;
        }
        this.Redrawable = true;
        this.Redraw();
        
        this.ScrollToStart();
    }
    
    protected override void OnClicked(int row, int column, SEElement element, Vector2 rawPos) {
        // check for which element is clicked
        // compare flag with at cursor.
        if (element != null && element.Definition.BlockDefName != null) {
            SEBlockDef blockDef = this.blockDefs[element.Definition.BlockDefName];
            if (blockDef.CompareFlags(this.cachedCursorFlags)) {
                this.ScriptPanelObject.CursorInsertBlock(blockDef);
            }
        }
    }
    
    public SEBlockDef.CompileFuncDelegate GetCompileFunc(string blockDefName) {
        return this.blockDefs[blockDefName].CompileFunc;
    }
    
    private SEElement GetPrefab(string elementType) {
        switch (elementType) {
            case "api":
                return this.SEAPIElementPrefab;
                break;
        
            default:
                return null;
                break;
        }
    }
    
}
