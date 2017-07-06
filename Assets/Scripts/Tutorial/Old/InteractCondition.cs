using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractCondition : MonoBehaviour, IInteractable {
    public string boolKey;
    public bool value;

    public void Interact(GameObject g)
    {
        Debug.Log("YAY");
        TutorialSystem.Current.SetOrAdd(boolKey, value);
    }
}
