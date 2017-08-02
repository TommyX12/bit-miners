using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToAction : LevelAction {

    public GameObject[] ToMove;
    public GameObject Target;
    public float speed;
    public static float SpeedMultiplier = 2.0f;
    
    public override void run()
    {
        base.run();
    }

    public override void PausingUpdate()
    {
        if (!running) {
            return;
        }

        bool checker = true;

        foreach (GameObject tm in ToMove) {
            if (!Util.MoveToTarget(tm, Target, speed * SpeedMultiplier)) {
                checker = false;
            }
        }

        if (checker) {
            isDone = true;
            running = false;
        }
    }

}
