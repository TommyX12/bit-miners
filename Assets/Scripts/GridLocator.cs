using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridLocator : MonoBehaviour {

    public bool debug;

    private GameObject indicator;
    public GameObject indicatorPrefab;

    private void Start()
    {
        indicator = GameObject.Instantiate(indicatorPrefab);
    }

    public void Update()
    {
        if (debug) {
            int[] gridPos =  Grid.instance.worldToGrid(transform.position);
            indicator.transform.position = Grid.instance.gridToWorld(gridPos[0], gridPos[1]);
        }
    }
}
