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
	
    public string ResourceType;
    public int ResourcesLeft;

	public MapData() {
		this.Collidable = false;
	}
	


	public void GenerateFromParam() {
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
		
		if (this.BiomeName == "water") {
			this.Collidable = true;
		}
		else {
			this.Collidable = false;
		}
		
		this.SpriteName = biome.SpriteName;

        // Tommy if this breaks go to RayScene and see how i set up my gamemanager
        ResourceType = GameManager.Current.ResourceSpawnManager.SpawnResource(BiomeName);
        Debug.Log(ResourceType);
        if (ResourceType != "nothing") {
            ResourcesLeft = 500;
        }
	}
}
