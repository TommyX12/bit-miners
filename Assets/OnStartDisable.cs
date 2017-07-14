using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnStartDisable : MonoBehaviour {
    public GameObject ToDisable;
    private void Start()
    {
        ToDisable.SetActive(false);
    }

}
