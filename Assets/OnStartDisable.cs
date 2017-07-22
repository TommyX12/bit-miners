using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnStartDisable : MonoBehaviour {
    public GameObject ToDisable;
    private void OnEnable()
    {
        ToDisable.SetActive(false);
    }

}
