using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : Unit {
    public int xSize;
    public int ySize;
    public GridCoord GridRef;

    private void OnDestroy()
    {

        int xorigin = GridRef.x;
        int yorigin = GridRef.y;

        for (int y = 0; y < ySize; y++)
        {
            for (int x = 0; x < xSize; x++)
            {
                Map.Current.Grid.Get(new GridCoord(xorigin + x, yorigin + y)).Occupied = true;
            }
        }
    }
}
