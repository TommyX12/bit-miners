using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class APIPanel: SEElementContainer {
    
    public SEAPIElement SEAPIElementPrefab;
    public ScriptPanel ScriptPanelObject;
    
    protected override void OnAwake() {
        
    }
    
    protected override void OnStart() {
        this.ScrollToStart();
    }
    
    public override void NormalUpdate() {
        base.NormalUpdate();
        
        if (this.gameObject.activeInHierarchy) {
            // check if script panel cursor region flag has changed.
            // if so, redraw self
        }
    }
    
    protected override void OnRedraw() {
        // compares flag for each element and set availability.
    }
    
    protected override void OnClicked(int row, int column, SEElement element, Vector2 rawPos) {
        // check for which element is clicked
        // compare flag with at cursor.
    }
    
    private Component GetPrefab(string elementType) {
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
