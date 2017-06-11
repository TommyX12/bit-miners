using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour {

    public static Grid instance;

    // size of the map. mapSize x mapSize tiles
    public int mapSize = 100;

    // base tile
    public GameObject tilePrefab;

    // hardcoded wallsprite
    public Sprite hcWallSprite;

    // Grid tiles
    Tile[,] tiles;

    // map creation is blocking for now. Can make it asynch later.
    // Generation is simple now, we can have fun with biomes and diversity later :)

    void Start()
    {
        instance = this;

        GameObject map = new GameObject();
        map.name = "map";

        tiles = new Tile[mapSize,mapSize];
        // Pools mapSize^2 tiles
        for (int i = 0; i < mapSize; i++) {
            for (int y = 0; y < mapSize; y ++) {
                tiles[i,y] = GameObject.Instantiate(tilePrefab).GetComponent<Tile>();
                Vector2 pos = gridToWorld(i, y);
                tiles[i, y].gameObject.transform.position = new Vector3(pos.x, pos.y, tiles[i, y].transform.position.z);

                // set edge tiles to unpathable
                if (i == 0 || i == mapSize - 1 || y == 0 || y == mapSize - 1) {
                    tiles[i, y].pathable = false;
                    tiles[i, y].tileSprite = hcWallSprite;
                }
                tiles[i, y].transform.SetParent(map.transform);
            }
        }
    }


    public Vector2 gridToWorld(int x, int y) {
        if (Input.GetKeyDown(KeyCode.E)) {
            Debug.Log(new Vector2(0.5f * x - ((float)mapSize / 2) * 0.5f, 0.5f * y - ((float)mapSize / 2) * 0.5f));
        }
        return new Vector2(0.5f * x - ((float)mapSize / 2) * 0.5f, 0.5f * y - ((float)mapSize / 2) * 0.5f);
    }

    public int[] worldToGrid(Vector2 pos) {
        int x = (int) ((pos.x + ((float)mapSize / 2) * 0.5f) * 2f + 0.25f);
        int y = (int) ((pos.y + ((float)mapSize / 2) * 0.5f) * 2f + 0.25f);
        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log(x + " " + y);
        }
        return new int[] { x, y };
    }
}
