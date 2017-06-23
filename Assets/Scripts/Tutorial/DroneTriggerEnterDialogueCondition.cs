using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneTriggerEnterDialogueCondition : MonoBehaviour {

    public string boolKey;
    public bool OnEnter;
    public bool OnExit;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.transform.root.CompareTag("Player")) {
            return;
        }
        TutorialSystem.Current.SetOrAdd(boolKey, OnEnter);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.transform.root.CompareTag("Player"))
        {
            return;
        }
        TutorialSystem.Current.SetOrAdd(boolKey, OnExit);
    }
}
