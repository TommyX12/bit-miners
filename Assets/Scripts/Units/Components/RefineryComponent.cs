using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefineryComponent : UnitComponent, IInteractable
{

    public float Multiplier;

    public int InputStore;
    public int OutputStore;

    private int InputStored;
    private int OutputStored;

    public float ProcessTime;
    private float ProcessTimer;

    public float BurnTimer;
    private float BurnTime;

    bool processing = false;

    public override void PausingFixedUpdate()
    {

        if (processing) {
            ProcessTimer -= ProcessTime;
            if (ProcessTimer < 0) {
                OutputStored += (int) (InputStored * Multiplier);
                if (OutputStored > OutputStore) {
                    OutputStored = OutputStore;
                }
            }
        }

        if (OutputStore > 0)
        {
            BurnTimer -= Time.fixedDeltaTime;
        }

        if (BurnTimer < 0)
        {
            OutputStored = 0;
            BurnTimer = BurnTime;
        }
    }

    public void Interact(GameObject g)
    {
        MiningComponent comp;
        if ((comp = (MiningComponent)g.GetComponent<Unit>().GetUnitComponent<MiningComponent>()) != null) {
            InputStored += comp.storage;
            comp.storage = 0;
        }
        if (InputStored > 0)
        {
            ProcessTimer = ProcessTime;
            processing = true;
        }
    }
}
