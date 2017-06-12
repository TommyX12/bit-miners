﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefineryComponent : UnitComponent, IInteractable
{
    public List<string> InputTypes;
    public List<int> InputAmounts;

    public List<string> OutputTypes;
    public List<int> OutputAmounts;

    public Dictionary<string, int> Inputs;
    public Dictionary<string, int> Outputs;
    public Dictionary<string, int> Stored;

    public float ProcessTime;
    private float ProcessTimer;

    bool processing = false;

    private void Start()
    {
        for (int i = 0; i < InputTypes.Count; i ++) {
            Inputs.Add(InputTypes[i], InputAmounts[i]);
            Outputs.Add(OutputTypes[i], OutputAmounts[i]);
            Stored.Add(OutputTypes[i], 0);
        }
    }

    public override void PausingFixedUpdate()
    {

        if (processing)
        {
            ProcessTimer -= Time.fixedDeltaTime;
            if (ProcessTimer < 0)
            {
                foreach (string key in new List<string>(Outputs.Keys)) {
                    Stored[key] = Outputs[key];
                }
                processing = false;
            }
        }
    }

    public void Interact(GameObject g)
    {
        MiningComponent comp;
        if ((comp = (MiningComponent)g.GetComponent<Unit>().GetUnitComponent<MiningComponent>()) != null)
        {
            if (Stored.ContainsKey(comp.type)) {
                if (Stored[comp.type] > comp.GetMaxStorage() - comp.GetStorage())
                {
                    int amt = comp.GetMaxStorage() - comp.storage;
                    Stored[comp.type] -= amt;
                    comp.storage += amt;
                }
                else {
                    comp.storage += Stored[comp.type];
                    Stored[comp.type] = 0;
                }
            }
        }
    }

    public void Process() {
        if (NewResourceManager.HasEnough(Inputs)) {
            NewResourceManager.Remove(Inputs);
            processing = true;
            ProcessTimer = ProcessTime;
        }
    }

    public bool IsProcessing() {
        return processing;
    }

    public int GetStored(string type) {
        return Stored[type];
    }

    public override void Register(ScriptSystem scriptSystem)
    {
        scriptSystem.RegisterFunction("get_stored", new Func<string, int>(GetStored));
        scriptSystem.RegisterFunction("process", new Action(Process));
        scriptSystem.RegisterFunction("is_processing", new Func<bool>(IsProcessing));
    }
}
