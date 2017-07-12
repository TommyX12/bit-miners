using System;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

[JsonObject(MemberSerialization.OptOut)]
public class SEElementDef: IIDBasedElememt {
    
    public delegate SEElement GetPrefabDelegate(string elementTyle);
    
    public int ID {
        get; set;
    }
    
    public int ParentID           = -1;
    public int[] Children         = null;
    public string BlockDefName    = null;
    public string ElementType     = null;
    public float[] Color          = new float[]{0, 0, 0, 1};
    public string Text            = "";
    public string RegionType      = "none";
    public string InputType      = null;
    public bool MultiRegion       = false;
    public int IndentMod          = 0;
    public bool ExtendSize         = false;
    [JsonIgnoreAttribute]
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
