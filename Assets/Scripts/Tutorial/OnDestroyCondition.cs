using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnDestroyCondition : MonoBehaviour {

    public string boolKey;
    public bool value;

    private void OnDestroy()
    {
        TutorialSystem.Current.SetOrAdd(boolKey, value);
    }
}
