using UnityEngine;
using System;
using System.Collections.Generic;

public class UnitSpawnPoint : SpawnPoint {
	
	public Unit UnitPrefab;
	public Unit SpawnedUnit = null;
	
	public UnitSpawnPoint(Unit prefab) {
		
	}
	
	public override void OnSpawn() {
		this.SpawnedUnit = (Unit)Util.Make(this.UnitPrefab);
		this.SpawnedUnit.SpawnPoint = this;
		this.SpawnedUnit.transform.position = this.Position;
		this.SpawnedUnit.transform.eulerAngles = new Vector3(0.0f, 0.0f, Util.RandomFloat(0.0f, 360.0f));
	}
	
	public override bool IsSpawned() {
		return this.SpawnedUnit != null && this.SpawnedUnit.Alive;
	}
	
	public bool IsUnitDestroyed() {
		return this.SpawnedUnit != null && !this.SpawnedUnit.Alive;
	}
	
	public void ReviveUnit() {
		this.SpawnedUnit = null;
	}
	
}
