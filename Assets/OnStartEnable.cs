using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnStartEnable : MonoBehaviour {
    public GameObject ToEnable;
    private void Start()
    {
        ToEnable.SetActive(true);
    }
}
