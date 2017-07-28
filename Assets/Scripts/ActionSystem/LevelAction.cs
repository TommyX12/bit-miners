using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelAction : MyMono {

    public bool isDone = false;
    public bool running = false;
    public LevelAction NextAction;

    public virtual void run() {
        running = true;
    }
}
