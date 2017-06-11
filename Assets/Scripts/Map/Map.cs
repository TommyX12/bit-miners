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
	}

	void Start() {
		this.GenerateMap();
	}
	
	public override void PausingUpdate() {
		this.Grid.Update(Camera.main);
	}
}
