using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateOnTriggerEnter : MonoBehaviour {

    public GameObject ToActivate;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        ToActivate.SetActive(true);
    }
}
