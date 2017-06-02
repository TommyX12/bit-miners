using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMiningComponent : UnitComponent {
    public float MiningRange = 0.25f;
    public float TimeMultiplier = 1f;
    public int MaxCapacity = 30;
    int currentCapacity = 0;
    float miningTimer;
    bool mining;
    Resource resource;
    Collider2D miningCollider;

    private void Start()
    {
        miningCollider = GetComponent<Collider2D>();
    }

    public void startMining()
    {
        if (mining)
        {
            return;
        }

        if (currentCapacity >= MaxCapacity)
        {
            return;
        }

        RaycastHit2D[] hitsBuffer = new RaycastHit2D[25];
        int hitCount = miningCollider.Cast(Vector2.zero, hitsBuffer);
        for (int i = 0; i < hitCount; i++)
        {
            if (hitsBuffer[i].collider.gameObject.CompareTag("Resource"))
            {
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
        if (mining)
        {
            miningTimer -= Time.fixedDeltaTime;
            if (miningTimer <= 0)
            {
                Debug.Log("Collected");
                mining = false;
                currentCapacity += resource.GetResource();
                if (currentCapacity >= MaxCapacity)
                {
                    currentCapacity = MaxCapacity;
                }

                if (((Vector2)(transform.position - resource.gameObject.transform.position)).magnitude > MiningRange)
                {
                    mining = false;
                    resource = null;
                }

            }
        }
    }

    public override void NormalUpdate()
    {
        base.NormalUpdate();
        if (Input.GetKey(KeyCode.E)) {
            startMining();
        }
    }
}
