using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionAction : LevelAction {
    public List<string> conditions;

    public override void run()
    {
        base.run();
        running = true;
    }

    public override void NormalFixedUpdate()
    {
        if (!running) {
            return;
        }

        if (ActionSystem.check(conditions)) {
            isDone = true;
            running = false;
        }
    }
}
