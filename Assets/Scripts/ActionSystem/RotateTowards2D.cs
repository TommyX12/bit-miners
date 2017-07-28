using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateTowards2D : LevelAction {
    public GameObject ToRotate;
    public GameObject Target;
    public float DegreesPerSecond;


    public override void run()
    {
        base.run();
        running = true;
    }

    public override void PausingFixedUpdate()
    {
        base.PausingFixedUpdate();
        if (!running) {
            return;
        }

        if (Util.RotateTowards2D(ToRotate, Target, DegreesPerSecond)) {
            running = false;
            isDone = true;
        }

    }
}
