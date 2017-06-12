using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorageComponent : UnitComponent, IInteractable {

    public string type;
    public int MaxCapacity;
    public int stored;
    public CircleCollider2D coll;

    public void Interact(GameObject g)
    {
        StorageComponent comp;
        if ((comp = (StorageComponent)g.GetComponent<Unit>().GetUnitComponent<StorageComponent>())) {
            if (comp.GetCurrentCapacity() > MaxCapacity - stored)
            {
                int amt = MaxCapacity - stored;
                comp.stored -= amt;
                stored += amt;
            }
            else {
                stored += comp.GetCurrentCapacity();
                comp.stored = 0;
            }
        }
    }

    public void TurnIn() {
        RaycastHit2D[] hits = new RaycastHit2D[20];
        int hitCount = coll.Cast(Vector2.zero, hits);
        for (int i = 0; i < hitCount; i++)
        {
            if (hits[i].collider.gameObject.GetComponent<IInteractable>() != null)
            {
                hits[i].collider.gameObject.GetComponent<IInteractable>().Interact(unit.gameObject);
            }
        }
    }

    public int GetMaxCapacity() {
        return MaxCapacity;
    }

    public int GetCurrentCapacity() {
        return stored;
    }

    public string GetResourceType() {
        return type;
    }

    public void SetType(string type) {
        this.type = type;
    }

    public override void Register(ScriptSystem scriptSystem)
    {
        scriptSystem.RegisterFunction("set_type", new Action<string>(SetType));
        scriptSystem.RegisterFunction("get_type", new Func<string>(GetResourceType));
        scriptSystem.RegisterFunction("get_current_capacity", new Func<int>(GetCurrentCapacity));
        scriptSystem.RegisterFunction("get_max_capacity", new Func<int>(GetMaxCapacity));
        scriptSystem.RegisterFunction("turn_in", new Action(TurnIn));
    }

}
