using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnEnableCondition : MonoBehaviour {
    public string boolKey;
    public bool value;

    private void OnEnable()
    {
        if (TutorialSystem.Current == null) {
            return;
        }
        TutorialSystem.Current.SetOrAdd(boolKey, value);
    }

}
