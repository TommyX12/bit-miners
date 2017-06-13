using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MapData {
	public float[] Color = new float[]{1.0f, 1.0f, 1.0f, 1.0f};
	public string SpriteName = "tile";
	
	public float BiomeParam1 = 0.0f; // precipitation 
	public float BiomeParam2 = 0.0f; // temperature
	public string BiomeName = "";
	
	public MapData() {
		
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
		
		this.SpriteName = biome.SpriteName;
	}
}
