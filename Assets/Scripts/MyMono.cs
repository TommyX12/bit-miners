using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyMono : MonoBehaviour {
    public static bool Paused = false;

    public void Update()
    {
        PausingUpdate();
        // other stuff
    }

    // this update returns when paused
    public virtual void PausingUpdate() {
        if (Paused) {
            return;
        }
    }
}
