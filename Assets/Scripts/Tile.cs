using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour {

    // A single tile should be 0.5x0.5 unity units

    // can units path through this tile?
    public bool pathable;

    // cost of moving to this tile. For pathfinding
    public int moveCost;

    // tile's sprite
    public Sprite tileSprite;
    
    // true if this tile can accept buildings
    public bool canBuildOn;

    // true if this tile has a building occupying it
    public bool occupied;

    // x location in tilegrid
    int x;
    // y location in tilegrid
    int y;

    // TODO: How to do multigrid tiles? :(

    private void Start()
    {

        // if not pathable, set collision to on
        if (!pathable) {
            GetComponent<Collider2D>().enabled = true;
        }

        // if an override sprite is given set it
        if (tileSprite != null) {
            GetComponent<SpriteRenderer>().sprite = tileSprite;
        }
    }
}
