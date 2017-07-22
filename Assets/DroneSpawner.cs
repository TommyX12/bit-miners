using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneSpawner : MyMono {

    public GameObject activeDrone;
    public GameObject DroneSpawnLocation;
    public GameObject DronePrefab;
    public bool shouldSpawn;
    public bool spawning;
    public MyAnimation anim;


    public override void PausingFixedUpdate()
    {
        base.PausingFixedUpdate();

        if (shouldSpawn) {
            anim.on = true;
            if (anim.picker >= 1) {
                shouldSpawn = false;
                spawning = true;
                activeDrone = GameObject.Instantiate(DronePrefab);
                activeDrone.transform.position = DroneSpawnLocation.transform.position;
                activeDrone.GetComponent<SpriteRenderer>().sortingLayerName = "Background";
            }
        }

        if (spawning) {
            if ((activeDrone.transform.position - transform.position).magnitude > 0.05)
            {
                activeDrone.transform.position += (transform.position - activeDrone.transform.position).normalized * Time.fixedDeltaTime;
            }
            else {
                spawning = false;
                anim.on = false;
                activeDrone.GetComponent<SpriteRenderer>().sortingLayerName = "default";
            }
        }

    }

    public void SpawnDrone() {
        Destroy(activeDrone);
        if (spawning == false)
        {
            shouldSpawn = true;
        }
    }

}
