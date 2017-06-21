using UnityEngine;
using System;
using System.Collections.Generic;

public class SpawnPoint {
	
	public Vector2 Position;
	
	public SpawnPoint() {
		
	}
	
	public void Spawn() {
		if (!this.IsSpawned()) {
			this.OnSpawn();
		}
	}
	
	public void Despawn() {
		if (this.IsSpawned()) {
			this.OnDespawn();
		}
	}
	
	public void Respawn() {
		this.Despawn();
		this.Spawn();
	}
	
	public virtual void OnSpawn() {
		
	}
	
	public virtual void OnDespawn() {
		
	}
	
	public virtual bool IsSpawned() {
		return false;
	}
	
}
