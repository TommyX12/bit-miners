using System;
using System.Collections.Generic;
using UnityEngine;

public class MapCollider : MyMono {
	
	public bool UseColliderIfPossible = true;
	public Collider2D Collider;
	public Rect LocalBoundingBox;
	public Map Map;
	
	void Awake() {
		if (this.Collider == null && this.UseColliderIfPossible) {
			this.Collider = this.GetComponent<Collider2D>();
		}
	}
	
	void Start() {
		if (this.Map == null) {
			this.Map = Map.Current;
		}
	}
	
	public override void PausingLateUpdate() {
		Rect worldBoundingBox;
		if (this.Collider == null) {
			worldBoundingBox = new Rect(new Vector2(this.transform.position.x, this.transform.position.y) + this.LocalBoundingBox.position, this.LocalBoundingBox.size);
		}
		else {
			Bounds bounds = this.Collider.bounds;
			worldBoundingBox = new Rect(bounds.min, bounds.max - bounds.min);
		}
		
		Vector2 position = this.transform.position;
		position += Grid.GetCollideOffset(this.Map.Grid, worldBoundingBox);
		this.transform.position = new Vector3(position.x, position.y, this.transform.position.z);
	}
	
}
