using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseWheelCondition : MyMono {

    public int ticks;
    public string key;
    public bool value;

    public override void PausingUpdate()
    {
        base.PausingUpdate();
        if (Input.mouseScrollDelta.y != 0)
        {
            ticks--;
        }
        if (ticks < 0)
        {
            TutorialSystem.Current.SetOrAdd(key, value);
            this.enabled = false;
        }
    }
}
