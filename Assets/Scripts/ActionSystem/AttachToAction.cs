using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachToAction : LevelAction {


    public GameObject ToAttach;
    public GameObject Parent;
    public Vector3 Offset;
    public bool UseCurrentPosition;
    public override void run()

    {
        base.run();

        if (!UseCurrentPosition)
        {
            ToAttach.transform.position = Parent.transform.position + Offset;
        }

        ToAttach.transform.SetParent(Parent.transform);

        this.running = false;
        this.isDone = true;
    }
}
