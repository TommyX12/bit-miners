using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SiloComponent : UnitComponent, IInteractable
{

    public int MaxCapacity;

    public string type;

    private void Start()
    {
        NewResourceManager.AddSilo(this);
    }

    private void OnDestroy()
    {
        NewResourceManager.RemoveSilo(this);
    }

    public void Interact(GameObject g)
    {
        StorageComponent miner = (StorageComponent) g.GetComponent<Unit>().GetUnitComponent<StorageComponent>();

        // type mismatch

        if (miner.type != type) {
            return;
        }

        if (NewResourceManager.GetMaxCapacity(type) - NewResourceManager.GetAmtStored(type) < miner.stored)
        {
            miner.stored -= NewResourceManager.GetMaxCapacity(type) - NewResourceManager.GetAmtStored(type);
            NewResourceManager.Add(type, NewResourceManager.GetMaxCapacity(type) - NewResourceManager.GetAmtStored(type));
        }
        else {
            NewResourceManager.Add(type, miner.stored);
            miner.stored = 0;
        }

    }
}
