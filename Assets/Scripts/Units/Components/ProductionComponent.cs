using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProductionComponent : UnitComponent {

    public GameObject SpawnLocation;

    public class BuildItem
    {
        public float time;
        public Dictionary<string, int> cost;
        public GameObject prefab;

        public void ProgressBuild(float progTime) {
            time -= progTime;
        }

    }

    public List<GameObject> BuildPrefabs; // limit this to 15
    public List<BuildItem> buildQueue; // max 5 build items at a time

    void Start()
    {
        if(buildQueue == null)
        buildQueue = new List<BuildItem>();
        if(BuildPrefabs == null)
        BuildPrefabs = new List<GameObject>();
    }

    public void Build(int id) {
        if (id >= BuildPrefabs.Count) {
            return;
            // TODO: error message
        }
        if (buildQueue.Count >= 5) {
            return;
            // TODO: error message
        }

        BuildItem bi = new BuildItem();
        Unit info = BuildPrefabs[id].GetComponent<Unit>();
        bi.time = info.BuildTime;
        Dictionary<string, int> cost = new Dictionary<string, int>();

        for (int i = 0; i < info.ResourceTypes.Count; i++) {
            cost.Add(info.ResourceTypes[i], info.ResourceCosts[i]);
        }

        bi.cost = cost;
        bi.prefab = BuildPrefabs[id];

        if (NewResourceManager.HasEnough(bi.cost)) {
            buildQueue.Add(bi);
            NewResourceManager.Remove(bi.cost);
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
        if (id >= buildQueue.Count) {
            return;
        }
        NewResourceManager.Add(buildQueue[id].cost);
        buildQueue.RemoveAt(id);
    }
        
    public override void Register(ScriptSystem scriptSystem)
    {
    }
}