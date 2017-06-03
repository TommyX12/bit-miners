using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SiloComponent : UnitComponent, IInteractable
{

    public int MaxCapacity;

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

        Debug.Log(miner.storage);
        if (ResourceManager.GetMaxCapacity() - ResourceManager.GetAmtStored() < miner.storage)
        {
            miner.storage -= ResourceManager.GetMaxCapacity() - ResourceManager.GetAmtStored();
            ResourceManager.Add(ResourceManager.GetMaxCapacity() - ResourceManager.GetAmtStored());
        }
        else {
            ResourceManager.Add(miner.storage);
            miner.storage = 0;
        }
    }
}
