using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapTile : MyMono, IGridElement<MapData> {
	
	public Grid<MapData> Grid{get; set;}
	public GridCoord Coord{get; set;}
	public Vector2 GridPos{get; set;}
	public MapData Data{get; set;}
	
	[HideInInspector]
	public SpriteRenderer SpriteRenderer;
	
	void Awake() {
		this.SpriteRenderer = this.GetComponent<SpriteRenderer>();
	}
	
	public void Activate() {
        Debug.Log("Active");
		this.gameObject.SetActive(true);
		
		this.SpriteRenderer.color = Util.Float4ToColor(this.Data.Color);
		this.SpriteRenderer.sprite = ResourceManager.GetSprite(this.Data.SpriteName);
		
		this.transform.localPosition = this.GridPos;

        // Tommy if this breaks go to RayScene and see how i set up my gamemanager
        GameManager.Current.ResourceSpawnManager.ShowResource(this);
	}
	
	public void Deactivate() {
		this.gameObject.SetActive(false);
	}
	
	public void Destroy() {
		Destroy(gameObject);
	}
	
	public override void PausingUpdate() {
		
	}
	
}
