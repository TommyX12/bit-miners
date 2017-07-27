using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelAction : MyMono {

    public bool isDone = false;
    public bool running = false;
    public bool restrictInput = false;

    public virtual void run() {
        running = true;
    }
}
