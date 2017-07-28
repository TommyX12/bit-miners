using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTriggerEnterCondition : MonoBehaviour {

    public string boolKey;
    public bool OnEnter;
    public bool OnExit;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.transform.root.CompareTag("Player"))
        {
            return;
        }
        ActionSystem.SetOrAdd(boolKey, OnEnter);
        this.enabled = false;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.gameObject.transform.root.CompareTag("Player"))
        {
            return;
        }
        ActionSystem.SetOrAdd(boolKey, OnExit);
    }

}
