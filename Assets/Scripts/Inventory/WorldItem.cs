using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class WorldItem : MyMono, IInteractable
{
    public SpriteRenderer rendy;
    private void Start()
    {
        rendy = GetComponent<SpriteRenderer>();
    }
    public void Interact(GameObject g)
    {
        Inventory.Current.Add(this);
    }
}
