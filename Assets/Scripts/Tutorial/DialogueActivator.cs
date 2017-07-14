using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueActivator : Dialogue {
    public GameObject[] ToActivate;

    public override void PreDisplay()
    {
        base.PreDisplay();
        foreach(GameObject g in ToActivate) { g.SetActive(true); }
    }
}
