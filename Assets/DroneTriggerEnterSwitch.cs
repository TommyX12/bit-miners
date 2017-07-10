using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneTriggerEnterSwitch : MonoBehaviour {

    public Switch sw;
    public bool OnEnter;
    public bool OnExit;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.transform.root.CompareTag("Player"))
        {
            return;
        }
        sw.on = OnEnter;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.transform.root.CompareTag("Player"))
        {
            return;
        }
        sw.on = OnExit;
    }
}
