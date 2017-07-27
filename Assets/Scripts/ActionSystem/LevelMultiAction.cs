using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelMultiAction : LevelAction {
    public List<LevelAction> actions;

    public override void PausingUpdate()
    {
        if (!running) {
            return;
        }
        bool checking = true;
        foreach (LevelAction a in actions) {
            if (!a.isDone) {
                checking = false;
            }
        }
        isDone = checking;
    }

    public override void run()
    {
        base.run();
        foreach (LevelAction a in actions) {
            a.run();
        }
    }

}
