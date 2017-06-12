using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MyMono {
	
	public Grid<MapData> Grid;
	
	public float TileSize = 0.5f;
	public int MapRadiusH = 64;
	public int MapRadiusV = 64;
	
	private MapTile ConstructElementFunc(Grid<MapData> grid) {
		MapTile tile = (MapTile)Util.MakeChild(this.transform, GameManager.Current.MapTilePrefab);
		return tile;
	}
	
	void Awake() {
		this.Grid = new Grid<MapData>(
			this.TileSize,
			true,
			3,
			this.transform,
			this.ConstructElementFunc
		);
	}
	
	private void GenerateMap() {
		this.Grid.Fill(this.MapRadiusH, this.MapRadiusV, null);
		
		foreach (GridCoord coord in Grid.Coords()){
			Grid.Set(coord, new MapData());
		}
		
		// Util.PushRandomSeed(Util.StringHash(SubjectName));
		
		// ArrayTexture2D mapBiomeParam1 = MapGenerator.generate_DiamondSquare(257, 0.35f, 0.65f, -0.4f, 0.4f, 0.75f);
		ArrayTexture2D mapBiomeParam1 = MapGenerator.generate_CaveCA(168, 168, 0.5f, 3);
		mapBiomeParam1.ReAverage(0.25f);
		this.Grid.ApplySampler(mapBiomeParam1, -MapRadiusH, MapRadiusH, -MapRadiusV, MapRadiusV, true, 
			delegate(Grid<MapData> grid, GridCoord coord, float pixel) {
				grid.Get(coord).Color[0] = pixel;
				grid.Get(coord).Color[1] = 0.85f;
				grid.Get(coord).Color[2] = 1.0f - pixel;
			}
		);
	}

	void Start() {
		this.GenerateMap();
	}
	
	public override void PausingUpdate() {
		this.Grid.Update(Camera.main);
	}
}
