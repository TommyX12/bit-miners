using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProductionComponent : UnitComponent {

    public GameObject SpawnLocation;

    public class BuildItem
    {
        public float time;
        public int cost;
        public GameObject prefab;

        public void ProgressBuild(float progTime) {
            time -= progTime;
        }

    }

    public List<GameObject> BuildPrefabs; // limit this to 4
    private List<BuildItem> buildQueue; // max 7 build items at a time

    private void Start()
    {
        buildQueue = new List<BuildItem>();
        BuildPrefabs = new List<GameObject>();
    }

    public void Build(int id) {
        if (id >= BuildPrefabs.Count) {
            return;
            // TODO: error message
        }
        if (buildQueue.Count >= 8) {
            return;
            // TODO: error message
        }

        BuildItem bi = new BuildItem();
        Unit info = BuildPrefabs[id].GetComponent<Unit>();
        bi.time = info.BuildTime;
        bi.cost = info.BuildCost;
        bi.prefab = BuildPrefabs[id];

        if (ResourceManager.HasEnough(bi.cost)) {
            buildQueue.Add(bi);
            ResourceManager.Remove(bi.cost);
        }
    }

    public override void PausingFixedUpdate()
    {
        base.PausingFixedUpdate();

        if (buildQueue.Count < 1) {
            return;
        }

        if (buildQueue[0].time <= 0)
        {

            GameObject obj = GameObject.Instantiate<GameObject>(buildQueue[0].prefab);

            if (SpawnLocation == null)
            {
                obj.transform.position = new Vector3(transform.position.x, transform.position.y, obj.transform.position.z);
            }
            else {
                obj.transform.position = new Vector3(SpawnLocation.transform.position.x, SpawnLocation.transform.position.y, obj.transform.position.z);
            }
            buildQueue.RemoveAt(0);
        } else {
            buildQueue[0].ProgressBuild(Time.fixedDeltaTime);
        }
    }

    public void Cancel(int id) {
        buildQueue.RemoveAt(id);
    }

}