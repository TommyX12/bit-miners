using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialoguePortraitAction : LevelAction {

    public string text;
    public Sprite portrait;

    public override void run()
    {
        base.run();
        DialoguePanel.current.FacialTransition(portrait, text);
    }

    public override void PausingFixedUpdate()
    {
        if (!running)
        {
            return;
        }

        if (DialoguePanel.current.isDone())
        {
            running = false;
            isDone = true;
        }
    }

}
