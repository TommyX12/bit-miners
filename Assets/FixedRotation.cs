using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedRotation : MonoBehaviour {

    private Quaternion rot;

	// Use this for initialization
	void Start () {
        rot = gameObject.transform.rotation;
	}

    private void LateUpdate()
    {
        gameObject.transform.rotation = rot;
    }
}
