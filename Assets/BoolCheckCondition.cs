using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoolCheckCondition : MonoBehaviour {

    public Object ScriptToCheck;
    public string BoolToCheck;
    public bool WantedValue;

    public string TutorialSystemBool;
    public bool Value;


    private void Update()
    {
        bool b = (bool)ScriptToCheck.GetType().GetField(BoolToCheck).GetValue(ScriptToCheck);
        if (b == WantedValue)
        {
            TutorialSystem.Current.SetOrAdd(TutorialSystemBool, Value);
            TutorialSystem.Current.next();
            this.enabled = false;
        }
    }
}
