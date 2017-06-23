using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnDisableCondition : MonoBehaviour {
    public string boolKey;
    public bool value;

    private void OnDisable()
    {
        TutorialSystem.Current.SetOrAdd(boolKey, value);
    }
}
