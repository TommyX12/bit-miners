using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Biome
{
	private static int _biomeMapWidth = 2;
	private static int _biomeMapHeight = 3;
	private static string[,] biomeMap = new string[,]{
		{"tundra", "taiga", "desert"},
		{"swamp", "grass_land", "rain_forest"},
	};
	
	public float[] Color = new float[]{1.0f, 1.0f, 1.0f, 1.0f};
	public string SpriteName = "tile";
	
	public Biome() {
		
	}
	
	private static Dictionary<string, Biome> biomeDict = new Dictionary<string, Biome>() {
		{"tundra", new Biome(){
			Color = Util.HexToFloat4("ccffff"),
		}},
		{"taiga", new Biome(){
			Color = Util.HexToFloat4("009999"),
		}},
		{"desert", new Biome(){
			Color = Util.HexToFloat4("ebbd4c"),
		}},
		{"swamp", new Biome(){
			Color = Util.HexToFloat4("5e5b16"),
		}},
		{"grass_land", new Biome(){
			Color = Util.HexToFloat4("80c13f"),
		}},
		{"rain_forest", new Biome(){
			Color = Util.HexToFloat4("32780b"),
		}},
	};

	public static Biome GetBiome(float param1, float param2) {
		param1 = Util.Clamp(param1, 0.0f, 0.999f);
		param2 = Util.Clamp(param2, 0.0f, 0.999f);
		
		int row = (int)(param1 * _biomeMapWidth);
		int column = (int)(param2 * _biomeMapHeight);
		
		return biomeDict[biomeMap[row, column]];
	}
}
