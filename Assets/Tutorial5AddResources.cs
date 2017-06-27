using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial5AddResources : MonoBehaviour {
    private void OnEnable()
    {
        NewResourceManager.Add("iron", 300);
    }
}
