using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floppy : WorldItem {

    private void Start()
    {
        init();
    }

    public override void init()
    {
        base.init();
        data = ExampleCode.Current.GetRandomCode();
    }

    public override void DroppedOnUnit(Unit unit)
    {
        base.DroppedOnUnit(unit);
        unit.ScriptSystemObject.Script = data;
        unit.StartEditor();
        Inventory.Current.Add(this);
    }
}
