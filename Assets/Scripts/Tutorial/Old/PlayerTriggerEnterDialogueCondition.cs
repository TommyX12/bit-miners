﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTriggerEnterDialogueCondition : MonoBehaviour {

    public string boolKey;
    public bool OnEnter;
    public bool OnExit;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.transform.root.CompareTag("Player"))
        {
            return;
        }
        TutorialSystem.Current.SetOrAdd(boolKey, OnEnter);
        TutorialSystem.Current.next();
        this.enabled = false;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.gameObject.transform.root.CompareTag("Player"))
        {
            return;
        }
        TutorialSystem.Current.SetOrAdd(boolKey, OnExit);
    }

}
