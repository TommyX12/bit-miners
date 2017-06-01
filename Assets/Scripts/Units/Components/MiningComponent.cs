using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiningComponent : UnitComponent {
    public float miningRange = 0.25f;
    public float timeMultiplier = 1f;
    float miningTimer;
    bool mining;
    GameObject resource;
    Collider2D miningCollider;

    public void startMining() {
        if (mining) {
            return;
        }

       // miningCollider.Cast(Vector2.zero)
    }
}
