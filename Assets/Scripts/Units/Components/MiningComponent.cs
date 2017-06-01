using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiningComponent : UnitComponent {
    public float MiningRange = 0.25f;
    public float TimeMultiplier = 1f;
    public int MaxCapacity = 30;
    int currentCapacity = 0;
    float miningTimer;
    bool mining;
    Resource resource;
    Collider2D miningCollider;

    public void startMining() {
        if (mining) {
            return;
        }

        if (currentCapacity >= MaxCapacity) {
            return;
        }

        RaycastHit2D[] hitsBuffer = new RaycastHit2D[25];
        int hitCount = miningCollider.Cast(Vector2.zero, hitsBuffer);
        for (int i = 0; i < hitCount; i++) {
            if (hitsBuffer[i].collider.gameObject.CompareTag("Resource")) {
                resource = hitsBuffer[i].collider.gameObject.GetComponent<Resource>();
                mining = true;
                miningTimer = resource.CollectionTime * TimeMultiplier;
                return;
            }
        }
    }

    public override void PausingFixedUpdate()
    {
        if (mining) {
            miningTimer -= Time.fixedDeltaTime;
            if (miningTimer <= 0) {
                mining = false;
                currentCapacity += resource.GetResource();
                if (currentCapacity >= MaxCapacity) {
                    currentCapacity = MaxCapacity;
                }

                if (((Vector2)(transform.position - resource.gameObject.transform.position)).magnitude > MiningRange) {
                    mining = false;
                    resource = null;
                }
            }
        }
    }
}
