using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapTile : MyMono, IGridElement<MapData> {
	
	public Grid<MapData> Grid{get; set;}
	public GridCoord Coord{get; set;}
	public Vector2 GridPos{get; set;}
	public MapData Data{get; set;}
	
	private SpriteRenderer SpriteRenderer;
	
	void Awake() {
		this.SpriteRenderer = this.GetComponent<SpriteRenderer>();
	}
	
	public void Activate() {
		this.SpriteRenderer.color = Util.Float4ToColor(this.Data.Color);
		this.SpriteRenderer.sprite = ResourceManager.GetSprite(this.Data.SpriteName);
		
		this.transform.localPosition = this.GridPos;
	}
	
	public void Deactivate() {
		
	}
	
	public void Destroy() {
		
	}
	
	public override void PausingUpdate() {
		
	}
	
}
