using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheatResources : MonoBehaviour {

    public string file;
    public TextAsset txt;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        NewResourceManager.Add("iron", 100);
	}
}
