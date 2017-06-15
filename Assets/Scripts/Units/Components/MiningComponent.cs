using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MiningComponent : StorageComponent {
    public float MiningRange = 0.25f;
    public float TimeMultiplier = 1f;
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

        if (stored >= MaxCapacity) {
            return;
        }

		// TODO fix mining
        /* RaycastHit2D[] hitsBuffer = new RaycastHit2D[25];
        int hitCount = miningCollider.Cast(Vector2.zero, hitsBuffer);
        for (int i = 0; i < hitCount; i++) {
            if (hitsBuffer[i].collider.gameObject.CompareTag("Resource") && hitsBuffer[i].collider.gameObject.GetComponent<Resource>().type == type) {
                resource = hitsBuffer[i].collider.gameObject.GetComponent<Resource>();
                mining = true;
                miningTimer = resource.CollectionTime * TimeMultiplier;
                return;
            }
        } */
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
                stored += resource.GetResource();
                if (stored >= MaxCapacity) {
                    stored = MaxCapacity;
                }
				// TODO fix mining
                /* if (((Vector2)(transform.position - resource.gameObject.transform.position)).magnitude > MiningRange) {
                    mining = false;
                    resource = null;
                } */

            }
        }
    }

    public double GetMiningRange() {
        return MiningRange;
    }

    public string GetMiningType() {
        return type;
    }

    public override void Register(ScriptSystem scriptSystem)
    {
        scriptSystem.RegisterFunction("set_type", new Action<string>(SetType));
        scriptSystem.RegisterFunction("get_type", new Func<string>(GetResourceType));
        scriptSystem.RegisterFunction("get_current_capacity", new Func<int>(GetCurrentCapacity));
        scriptSystem.RegisterFunction("get_max_capacity", new Func<int>(GetMaxCapacity));
        scriptSystem.RegisterFunction("turn_in", new Action(TurnIn));
        scriptSystem.RegisterFunction("mine", new Action(startMining));
        scriptSystem.RegisterFunction("get_mining_range", new Func<double>(GetMiningRange));
    }

}
