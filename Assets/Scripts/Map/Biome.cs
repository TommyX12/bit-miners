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
	
	private static Dictionary<string, Biome> biomeDict = new Dictionary<string, Biome>() {
		{"tundra", new Biome(){
			Color = Util.HexToFloat4("#ccddff"),
		}},
		{"water", new Biome(){
			Color = Util.HexToFloat4("#147caa"),
		}},
		{"desert", new Biome(){
			Color = Util.HexToFloat4("#ebbd4c"),
		}},
		{"swamp", new Biome(){
			Color = Util.HexToFloat4("#504026"),
		}},
		{"grass_land", new Biome(){
			Color = Util.HexToFloat4("#80c13f"),
		}},
		{"rain_forest", new Biome(){
			Color = Util.HexToFloat4("#30750b"),
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
