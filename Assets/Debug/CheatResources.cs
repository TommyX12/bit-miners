using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheatResources : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        NewResourceManager.Add("iron", 100);
	}
}
