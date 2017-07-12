using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class APIPanel: SEElementContainer {
    
    public SEAPIElement SEAPIElementPrefab;
    public ScriptPanel ScriptPanelObject;
    
    private int cachedCursorFlags = -1;
    
    private Dictionary<string, SEBlockDef> blockDefs = null;
    
    protected override void OnAwake() {
        
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
        foreach (var row in this.data) {
            SEBlockDef blockDef = this.blockDefs[row[0].Definition.BlockDefName];
            row[0].SetActive(blockDef.CompareFlags(this.cachedCursorFlags));
        }
    }
    
    protected override void OnRedraw() {
        this.RefreshFlags();
    }
    
    public void LoadBlockDefs(Dictionary<string, SEBlockDef> blockDefs) {
        this.ClearElements();
        this.blockDefs = blockDefs;
        
        this.Redrawable = false;
        int row = 0;
        foreach (var blockDef in blockDefs.Values) {
            this.InsertElement(row, 0, blockDef.GenerateAPIElementDef().SpawnElement(this.GetPrefab));
            row++;
        }
        this.Redrawable = true;
        this.Redraw();
        
        this.ScrollToStart();
    }
    
    protected override void OnClicked(int row, int column, SEElement element, Vector2 rawPos) {
        // check for which element is clicked
        // compare flag with at cursor.
        if (element != null) {
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
