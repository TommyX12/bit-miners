using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialDroneSpawner : MonoBehaviour {
    public MyAnimation anim;
    public GameObject toMove;

    public void SpawnDrone() {
        toMove.transform.position = this.transform.position;
        anim.on = true;
    }

}
