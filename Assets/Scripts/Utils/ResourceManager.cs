using System.Collections.Generic;
using System;
using UnityEngine;

public class ResourceManager {
	
	private static Dictionary<string, Sprite> sprites;
    private static List<string> code;

	public static void Initialize() {
		Sprite[] spriteObjects = Resources.LoadAll<Sprite>("Sprites");
		sprites = new Dictionary<string, Sprite>();
		foreach (Sprite sprite in spriteObjects) {
			sprites[sprite.name] = sprite;
		}
	}
	
	public static Sprite GetSprite(string name) {
		Sprite result;
		sprites.TryGetValue(name, out result);
		return result;
	}
	
}
