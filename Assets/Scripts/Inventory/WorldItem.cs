using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class WorldItem : MyMono, IInteractable
{

    [HideInInspector]
    public string data;

    public SpriteRenderer rendy;

    private void Start()
    {
        init();
    }

    public virtual void init() {
        rendy = GetComponent<SpriteRenderer>();
    }

    public virtual void Interact(GameObject g)
    {
        Inventory.Current.Add(this);
    }

    public virtual void DroppedOnUnit(Unit unit) {

    }
}
