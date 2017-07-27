using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableDisableScriptAction : LevelAction {

    Behaviour[] targets;

    bool active;
    public override void run()
    {
        base.run();
        foreach (Behaviour g in targets)
        {
            g.enabled = active;
        }

        running = false;
        isDone = true;
    }

}
