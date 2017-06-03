using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputCaratFix : MonoBehaviour {

    public GameObject contentInputCaret;
    public GameObject Content;

    bool jobDone = false;
	
	// Update is called once per frame
	void Update () {
        if (jobDone) {
            return;
        }

        if (GameObject.Find("Content Input Caret") != null) {
            contentInputCaret = GameObject.Find("Content Input Caret");
            contentInputCaret.transform.SetParent(Content.transform);
            jobDone = true;
        }	
	}
}
