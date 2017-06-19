using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MapData : IGridCollidable {
	public float[] Color = new float[]{1.0f, 1.0f, 1.0f, 1.0f};
	public string SpriteName = "tile";
	
	public float BiomeParam1 = 0.0f; // precipitation 
	public float BiomeParam2 = 0.0f; // temperature
	public string BiomeName = "";
	
	public bool Occupied = false;
	public bool Collidable {
		get; set;
	}
	
	public bool Modified = true;
	
	public Resource ResourceObject = null;

	public MapData() {
		this.Collidable = false;
	}
	
	public void RemoveResource() {
		this.Modified = true;
		
		this.ResourceObject = null;
		this.Collidable = false;
        Occupied = false;
	}

	public void GenerateFromParam() {
		this.Modified = true;
		
		this.BiomeParam1 = Util.Clamp(this.BiomeParam1, 0.0f, 1.0f);
		this.BiomeParam2 = Util.Clamp(this.BiomeParam2, 0.0f, 1.0f);

		Biome biome = Biome.GetBiome(this.BiomeParam1, this.BiomeParam2);
		
		this.BiomeName = biome.Name;
		
		this.Color[0] = biome.Color[0];
		this.Color[1] = biome.Color[1];
		this.Color[2] = biome.Color[2];
		this.Color[3] = 1.0f;
		
		this.Color[0] *= 1.0f - (0.75f * this.BiomeParam1);
		this.Color[1] *= 1.0f - (0.35f * this.BiomeParam2);
		this.Color[2] *= 1.0f - (0.75f * this.BiomeParam2);
		
		this.Collidable = false;
		
		this.SpriteName = biome.SpriteName;

		// resource spawning
		string resourceType = Util.RandomSelectChance(biome.ResourceSpawnChance);
		if (resourceType != null && resourceType != "nothing") {
			ResourceObject = Resource.Construct(resourceType, this);
			this.Collidable = true;
            Occupied = true;
		}
	}
}
