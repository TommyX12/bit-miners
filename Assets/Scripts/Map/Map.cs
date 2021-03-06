﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MyMono {
    
    public static Map Current;
    public Grid<MapData> Grid;
    
    public static float TileSize = 0.5f;
    public static int MapRadiusH = 64;
    public static int MapRadiusV = 64;
    public static int SafeZoneRadius = 10; 
    
    public HashSet<UnitSpawnPoint> UnitSpawnPoints = new HashSet<UnitSpawnPoint>();
    
    private MapTile ConstructElementFunc(Grid<MapData> grid) {
        MapTile tile = (MapTile)Util.MakeChild(this.transform, GameManager.Current.MapTilePrefab);
        return tile;
    }
    
    void Awake() {
        Current = this;
        
        this.Grid = new Grid<MapData>(
            TileSize,
            true,
            2,
            this.transform,
            this.ConstructElementFunc
        );
    }
    
    private void GenerateMap() {
        // Util.PushRandomSeed(Util.StringHash(SubjectName));
        
        { // init grid
            this.Grid.Fill(MapRadiusH, MapRadiusV, null);
            
            foreach (GridCoord coord in Grid.Coords()){
                Grid.Set(coord, new MapData());
            }
        }
        
        { // generate biome parameter
            ArrayTexture2D mapDS1 = MapGenerator.generate_DiamondSquare(257, 0.35f, 0.65f, -0.4f, 0.4f, 0.75f);
            mapDS1.ReAverage(0.25f);
            ArrayTexture2D mapDS2 = MapGenerator.generate_DiamondSquare(257, 0.30f, 0.70f, -0.3f, 0.3f, 0.75f);
            mapDS2.ReAverage(0.25f);
            ArrayTexture2D mapRW = MapGenerator.generate_RandomWalk(128, 128, 1.0f, 64, 64, 8192, -0.3f, true);
            ArrayTexture2D mapCA = MapGenerator.generate_CaveCA(168, 168, 0.5f, 3);
            
            this.Grid.ApplySampler(mapDS1, -MapRadiusH, MapRadiusH, -MapRadiusV, MapRadiusV, true, 
                delegate(Grid<MapData> grid, GridCoord coord, float pixel) {
                    MapData mapData = grid.Get(coord);
                    mapData.BiomeParam1 = pixel;
                }
            );
            
            this.Grid.ApplySampler(mapDS2, -MapRadiusH, MapRadiusH, -MapRadiusV, MapRadiusV, true, 
                delegate(Grid<MapData> grid, GridCoord coord, float pixel) {
                    MapData mapData = grid.Get(coord);
                    mapData.BiomeParam2 = pixel;
                }
            );
            
            foreach (MapData mapData in this.Grid.Data()) {
                mapData.GenerateFromParam();
            }
        }
        
        { // safe zone
            GridCoord playerCoord = this.Grid.PointToCoord(GameManager.Current.Player.transform.position);
            foreach (GridCoord coord in this.Grid.Range(SafeZoneRadius, 1, playerCoord)) {
                MapData data = this.Grid.Get(coord);
                if (data != null) {
                    data.UnitSpawnPoints.Clear();
                }
            }
        }
        
        { // register spawn points
            this.UnitSpawnPoints = new HashSet<UnitSpawnPoint>();
            foreach (MapData data in this.Grid.Data()) {
                if (data != null) {
                    foreach (UnitSpawnPoint spawnPoint in data.UnitSpawnPoints) {
                        this.UnitSpawnPoints.Add(spawnPoint);
                    }
                }
            }
        }
        
        // Util.PopRandomSeed();
    }
    
    private void SpawnAll() {
        foreach (UnitSpawnPoint spawnPoint in this.UnitSpawnPoints) {
            spawnPoint.Spawn();
        }
    }

    void Start() {
        this.GenerateMap();
        this.SpawnAll();
    }
    
    public override void PausingUpdate() {
        this.Grid.Update(Camera.main);
    }
}
