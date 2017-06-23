using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueConditionActivator : Dialogue {

    public string boolKey;
    public bool value;

    public override void PostDisplay()
    {
        TutorialSystem.Current.SetOrAdd(boolKey, value);
    }

}
