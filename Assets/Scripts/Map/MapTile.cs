using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapTile : MyMono, IGridElement<MapData> {
	
	public Grid<MapData> Grid{get; set;}
	public GridCoord Coord{get; set;}
	public Vector2 GridPos{get; set;}
	public MapData Data{get; set;}
	
	public void Activate() {
		this.transform.localPosition = this.GridPos;
	}
	
	public void Deactivate() {
		
	}
	
	public void Destroy() {
		
	}
	
	public override void PausingUpdate() {
		
	}
	
}
