using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueCharacterAction : LevelAction {

    public string text;
    public string charname;
    public Sprite portrait;

    public override void run()
    {
        base.run();
        DialoguePanel.current.CharacterTransition(portrait, charname, text);
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
