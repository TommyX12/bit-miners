using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapTile : MyMono, IGridElement<MapData> {
	
	public Grid<MapData> Grid{get; set;}
	public GridCoord Coord{get; set;}
	public Vector2 GridPos{get; set;}
	public MapData Data{get; set;}
	
	public SpriteRenderer BackgroundSprite;
	public SpriteRenderer ForegroundSprite;
	
	void Awake() {
		this.BackgroundSprite = this.transform.Find("BackgroundSprite").GetComponent<SpriteRenderer>();
		this.ForegroundSprite = this.transform.Find("ForegroundSprite").GetComponent<SpriteRenderer>();
	}
	
	public void Activate() {
		if (this.Data == null) return;
		
		this.gameObject.SetActive(true);
		
		this.BackgroundSprite.color = Util.Float4ToColor(this.Data.Color);
		this.BackgroundSprite.sprite = ResourceManager.GetSprite(this.Data.SpriteName);
		
		if (this.Data.ResourceObject != null) {
			this.ForegroundSprite.gameObject.SetActive(true);
			this.ForegroundSprite.sprite = ResourceManager.GetSprite(this.Data.ResourceObject.SpriteName);
		}
		else {
			this.ForegroundSprite.gameObject.SetActive(false);
		}
		
		this.transform.localPosition = this.GridPos;
	}
	
	public void Deactivate() {
		this.gameObject.SetActive(false);
	}
	
	public void Destroy() {
		Destroy(gameObject);
	}
	
	public override void PausingUpdate() {
		if (this.Data == null) return;
		
		if (this.Data.Modified) {
			this.Activate();
			this.Data.Modified = false;
		}
	}
	
}
