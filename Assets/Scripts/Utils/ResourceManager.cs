using System.Collections.Generic;
using System;
using UnityEngine;

public class ResourceManager {
    
    private static Dictionary<string, Sprite> sprites;
    private static Dictionary<string, AudioClip> music;
    private static Dictionary<string, string> texts;
    private static List<string> code;
    
    private static bool initialized = false;

    private static void Initialize() {
        if (initialized) return;
        initialized = true;
        
        Sprite[] spriteObjects = Resources.LoadAll<Sprite>("Sprites");
        sprites = new Dictionary<string, Sprite>();
        foreach (Sprite sprite in spriteObjects) {
            sprites[sprite.name] = sprite;
        }
        
        AudioClip[] musicObjects = Resources.LoadAll<AudioClip>("Music");
        music = new Dictionary<string, AudioClip>();
        foreach (AudioClip clip in musicObjects) {
            music[clip.name] = clip;
        }
        
        TextAsset[] textObjects = Resources.LoadAll<TextAsset>("Data");
        texts = new Dictionary<string, string>();
        foreach (TextAsset text in textObjects) {
            texts[text.name] = text.text;
        }
    }
    
    public static Sprite GetSprite(string name) {
        Initialize();
        Sprite result;
        sprites.TryGetValue(name, out result);
        return result;
    }
    
    public static AudioClip GetMusic(string name) {
        Initialize();
        AudioClip result;
        music.TryGetValue(name, out result);
        return result;
    }
    
    public static string GetText(string name) {
        Initialize();
        string result;
        texts.TryGetValue(name, out result);
        return result;
    }
    
}
