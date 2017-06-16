using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MiningComponent : StorageComponent {
    public float MiningRange = 0.25f;
    public float TimeMultiplier = 1f;
    public float miningTimer;
    public bool mining;
    public ParticleSystem sparker;
    public MiningSpriteManager spriteManager;
    Resource resource;
    GridCoord resourceCoordinate;

    public float calltimer;

    public void startMining() {
        if (mining || calltimer > 0)
        {
            return;
        }

        if (stored >= MaxCapacity)
        {
            return;
        }

        GridCoord current = Map.Current.Grid.PointToCoord(unit.transform.position);
        foreach (GridCoord gc in Map.Current.Grid.Neighbors(current)) {

            Debug.DrawLine(Map.Current.Grid.CoordToPoint(gc), Vector3.zero, Color.white, 1);

            if ((resource = Map.Current.Grid.GetElement(gc).Data.ResourceObject) != null && resource.type == type) {
                Debug.Log(resource.type);
                Debug.DrawLine(Map.Current.Grid.CoordToPoint(gc), Vector3.zero, Color.green, 1);
                resourceCoordinate = gc;
                mining = true;
                calltimer = 1;
                miningTimer = resource.CollectionTime * TimeMultiplier;
                return;
            }

        }

        calltimer = 1;
        return;

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
        calltimer -= Time.fixedDeltaTime;
        if (mining)
        {
            sparker.Play();
            miningTimer -= Time.fixedDeltaTime;
            if (resource == null)
            {
                mining = false;
                return;
            }

            if (miningTimer <= 0)
            {
                mining = false;
                stored += resource.GetResource();
                if (stored >= MaxCapacity)
                {
                    stored = MaxCapacity;
                }
                GridCoord current = Map.Current.Grid.PointToCoord(unit.transform.position);
                if (Grid<MapData>.ManhattanDistance(current, resourceCoordinate) > MiningRange)
                {
                    mining = false;
                    resource = null;
                }
                spriteManager.Refresh();
            }
        }
        else {
            sparker.Pause();
        }
    }

    public double GetMiningRange() {
        return MiningRange;
    }

    public string GetMiningType() {
        return type;
    }

    public override void TurnIn()
    {
        base.TurnIn();
        spriteManager.Refresh();
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
