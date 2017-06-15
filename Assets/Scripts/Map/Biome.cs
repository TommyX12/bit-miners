using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Biome
{
	private static int _biomeMapWidth = 2;
	private static int _biomeMapHeight = 3;
	private static string[,] biomeMap = new string[,]{
		{"tundra", "grass_land", "desert"},
		{"water", "swamp", "rain_forest"},
	};
	
	public float[] Color = new float[]{1.0f, 1.0f, 1.0f, 1.0f};
	public string SpriteName = "tile";
	public string Name = "";
	public Dictionary<string, float> ResourceSpawnChance = new Dictionary<string, float>();

    private static Dictionary<string, Biome> biomeDict = new Dictionary<string, Biome>() {
        {"tundra", new Biome(){
            Color = Util.HexToFloat4("#ccddff"),
            Name = "tundra",
			ResourceSpawnChance = new Dictionary<string, float>() {
				{"iron", 1f},
				{"gold", 1f},
				{"oil", 2f},
				{"coal", 0f},
				{"nothing", 997f}
			},
		}},
		{"water", new Biome(){
			Color = Util.HexToFloat4("#147caa"),
			Name = "water",
			ResourceSpawnChance = new Dictionary<string, float>() {
				{"iron", 0f},
				{"gold", 0f},
				{"oil", 5f},
				{"coal", 0f},
				{"nothing", 995f}
			},
        }},
		{"desert", new Biome(){
			Color = Util.HexToFloat4("#ebbd4c"),
            Name = "desert",
			ResourceSpawnChance = new Dictionary<string, float>() {
				{"iron", 0f},
				{"gold", 0f},
				{"oil", 10f},
				{"coal", 0f},
				{"nothing", 990f}
			},
        }},
		{"swamp", new Biome(){
			Color = Util.HexToFloat4("#504026"),
            Name = "swamp",
			ResourceSpawnChance = new Dictionary<string, float>() {
				{"iron", 3f},
				{"gold", 9f},
				{"oil", 0f},
				{"coal", 10f},
				{"nothing", 987f}
			},
        }},
		{"grass_land", new Biome(){
			Color = Util.HexToFloat4("#80c13f"),
            Name = "grass_land",
			ResourceSpawnChance = new Dictionary<string, float>() {
				{"iron", 10f},
				{"gold", 2f},
				{"oil", 0f},
				{"coal", 5f},
				{"nothing", 983f}
			},
        }},
		{"rain_forest", new Biome(){
			Color = Util.HexToFloat4("#30750b"),
            Name = "rain_forest",
			ResourceSpawnChance = new Dictionary<string, float>() {
				{"iron", 5f},
				{"gold", 10f},
				{"oil", 2f},
				{"coal", 3f},
				{"nothing", 980f}
			},
        }},
	};
	
	static Biome() {
		foreach (var item in biomeDict) {
			item.Value.Name = item.Key;
		}
	}

	public Biome() {
		
	}
	
	public static Biome GetBiome(float param1, float param2) {
		param1 = Util.Clamp(param1, 0.0f, 0.999f);
		param2 = Util.Clamp(param2, 0.0f, 0.999f);
		
		int row = (int)(param1 * _biomeMapWidth);
		int column = (int)(param2 * _biomeMapHeight);
		
		return biomeDict[biomeMap[row, column]];
	}
	
	public static Biome GetBiome(string name) {
		return biomeDict[name];
	}
}
