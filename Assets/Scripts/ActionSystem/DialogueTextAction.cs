using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTextAction : LevelAction {

    public string text;

    public override void run()
    {
        base.run();
        DialoguePanel.current.TextTransition(text);
    }

    public override void PausingFixedUpdate()
    {
        if (!running) {
            return;
        }
        if (DialoguePanel.current.isDone()) {
            running = false;
            isDone = true;
        }
    }
}
