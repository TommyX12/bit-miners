using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableAfterStart : MonoBehaviour {

    public string boolKey;

    private void Start()
    {
        gameObject.SetActive(false);
    }

    private void Update()
    {
        if (TutorialSystem.Current.check(new List<string>() { boolKey }) == false) {
            gameObject.SetActive(false);
        }
    }

}
