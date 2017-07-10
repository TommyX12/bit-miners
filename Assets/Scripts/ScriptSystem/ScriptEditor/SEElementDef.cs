using System;
using System.Collections.Generic;
using UnityEngine;

public class SEElementDef: IIDBasedElememt {
    
    public delegate Component GetPrefabDelegate(string elementTyle);
    
    public int ID {
        get; set;
    }
    
    public int ParentID           = -1;
    public int[] Children         = null;
    public string CompileFuncName = null;
    public string ElementType     = null;
    public float[] Color          = new float[]{0, 0, 0, 1};
    public float[] TextColor      = new float[]{1, 1, 1, 1};
    public string Text            = null;
    public string RegionType      = null;
    public int IndentMod          = 0;
    public SEElement Element      = null;
    
    public SEElementDef() {
        this.ID = -1;
    }
    
    public SEElement SpawnElement(GetPrefabDelegate getPrefabFunc) {
        SEElement element = (SEElement)Util.Make(getPrefabFunc(this.ElementType));
        element.Definition = (SEElementDef)this.MemberwiseClone();
        element.Definition.Element = element;
        element.SetupDefinition();
        return element;
    }
    
}
