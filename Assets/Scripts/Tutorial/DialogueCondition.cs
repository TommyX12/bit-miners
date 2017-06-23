using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueCondition : MonoBehaviour {

    public string boolKey;
    public bool value;

    public void Activate() {
        TutorialSystem.Current.SetOrAdd(boolKey, value);
    }

}
