using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floppy : WorldItem {

    public bool autoGen = true;
    
    private void Start()
    {
        init();
    }

    public override void init()
    {
        base.init();
        if (autoGen)
        {
            data = ExampleCode.Current.GetRandomCode();
        }
    }

    public override void DroppedOnUnit(Unit unit)
    {
        unit.ScriptSystemObject.Script = data;
        unit.StartEditor();
        Inventory.Current.Add(this);
    }
}
