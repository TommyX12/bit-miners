using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HardFloppy : Floppy {
    public TextAsset source;

    private void Start()
    {
        init();
    }

    public override void init()
    {
        base.init();
    }

    public override void DroppedOnUnit(Unit unit)
    {
        base.DroppedOnUnit(unit);
        unit.ScriptSystemObject.Script = source.text;
        unit.StartEditor();
        Inventory.Current.Add(this);
    }
}
