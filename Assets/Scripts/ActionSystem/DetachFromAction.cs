using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetachFromAction : LevelAction {

    public GameObject ToDetach;

    public override void run()
    {
        base.run();
        ToDetach.transform.SetParent(null);
        isDone = true;
    }


}
