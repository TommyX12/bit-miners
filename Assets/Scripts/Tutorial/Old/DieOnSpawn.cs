using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieOnSpawn : MonoBehaviour {

    public float delay;
    public GameObject toKill;
    private void Start()
    {
        Destroy(toKill, delay);
    }
}
