using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableDisableHackingAction : LevelAction {

    public bool Enable;
    public override void run()
    {
        base.run();
        
        GameManager.Current.HackEnabled = this.Enable;

        running = false;
        isDone = true;
    }
    
}
