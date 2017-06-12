using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MiningComponent : UnitComponent {
    public float MiningRange = 0.25f;
    public float TimeMultiplier = 1f;
    public int MaxCapacity = 30;
    public int storage = 0;
    public string type;
    float miningTimer;
    bool mining;
    Resource resource;
    CircleCollider2D miningCollider;

    private void Start()
    {
        miningCollider = GetComponent<CircleCollider2D>();
        miningCollider.radius = MiningRange;
    }

    public void startMining() {
        if (mining) {
            return;
        }

        if (storage >= MaxCapacity) {
            return;
        }

        RaycastHit2D[] hitsBuffer = new RaycastHit2D[25];
        int hitCount = miningCollider.Cast(Vector2.zero, hitsBuffer);
        for (int i = 0; i < hitCount; i++) {
            if (hitsBuffer[i].collider.gameObject.CompareTag("Resource") && hitsBuffer[i].collider.gameObject.GetComponent<Resource>().type == type) {
                resource = hitsBuffer[i].collider.gameObject.GetComponent<Resource>();
                mining = true;
                miningTimer = resource.CollectionTime * TimeMultiplier;
                return;
            }
        }
    }

    public override void PausingFixedUpdate()
    {
        base.PausingFixedUpdate();
        if (mining) {
            miningTimer -= Time.fixedDeltaTime;

            if (resource == null) {
                mining = false;
                return;
            }

            if (miningTimer <= 0) {
                Debug.Log("Collected");
                mining = false;
                storage += resource.GetResource();
                if (storage >= MaxCapacity) {
                    storage = MaxCapacity;
                }
                if (((Vector2)(transform.position - resource.gameObject.transform.position)).magnitude > MiningRange) {
                    mining = false;
                    resource = null;
                }

            }
        }
    }

    public void TurnIn() {
        RaycastHit2D[] hits = new RaycastHit2D[20];
        int hitCount = miningCollider.Cast(Vector2.zero, hits);
        for (int i = 0; i < hitCount; i++)
        {
            if (hits[i].collider.gameObject.GetComponent<IInteractable>() != null)
            {
                hits[i].collider.gameObject.GetComponent<IInteractable>().Interact(unit.gameObject);
            }
        }
    }

    public int GetMaxStorage() {
        return MaxCapacity;
    }

    public int GetStorage() {
        return storage;
    }

    public double GetMiningRange() {
        return MiningRange;
    }

    public void ChangeType(string type) {
        storage = 0;
        this.type = type;
    }

    public string GetMiningType() {
        return type;
    }

    public override void Register(ScriptSystem scriptSystem)
    {
        scriptSystem.RegisterFunction("get_max_storage", new Func<int>(GetMaxStorage));
        scriptSystem.RegisterFunction("get_storage", new Func<int>(GetStorage));
        scriptSystem.RegisterFunction("mine", new Action(startMining));
        scriptSystem.RegisterFunction("turn_in", new Action(TurnIn));
        scriptSystem.RegisterFunction("get_mining_range", new Func<double>(GetMiningRange));
        scriptSystem.RegisterFunction("change_type", new Action<string>(ChangeType));
        scriptSystem.RegisterFunction("get_type", new Func<string>(GetMiningType));
    }

}
