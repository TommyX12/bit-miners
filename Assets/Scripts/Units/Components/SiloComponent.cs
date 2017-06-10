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
        ResourceManager.AddSilo(this);
    }

    private void OnDestroy()
    {
        ResourceManager.RemoveSilo(this);
    }

    public void Interact(GameObject g)
    {
        MiningComponent miner = (MiningComponent) g.GetComponent<Unit>().GetUnitComponent<MiningComponent>();

        // type mismatch

        if (miner.type != type) {
            return;
        }

        if (NewResourceManager.GetMaxCapacity(type) - NewResourceManager.GetAmtStored(type) < miner.storage)
        {
            miner.storage -= NewResourceManager.GetMaxCapacity(type) - NewResourceManager.GetAmtStored(type);
            NewResourceManager.Add(type, NewResourceManager.GetMaxCapacity(type) - NewResourceManager.GetAmtStored(type));
        }
        else {
            NewResourceManager.Add(type, miner.storage);
            miner.storage = 0;
        }

    }
}
