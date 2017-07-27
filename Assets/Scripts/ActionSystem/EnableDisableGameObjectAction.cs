using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableDisableGameObjectAction : LevelAction {

    public GameObject[] targets;
    bool active;
    public override void run()
    {
        base.run();
        foreach (GameObject g in targets) {
            g.SetActive(active);
        }

        running = false;
        isDone = true;
    }

}
