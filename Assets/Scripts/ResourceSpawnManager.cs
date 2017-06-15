using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class ResourceSpawnManager : MonoBehaviour {

    public GameObject ironPrefab;
    public GameObject goldPrefab;
    public GameObject oilPrefab;
    public GameObject coalPrefab;

    // tundra

    static Dictionary<string, float> tundra = new Dictionary<string, float>()
    {
        {"iron", 1f},
        {"gold", 1f},
        {"oil", 2f},
        {"coal", 0f},
        {"nothing", 997f}
    };

    // water

    static Dictionary<string, float> water = new Dictionary<string, float>()
    {
        {"iron", 0f},
        {"gold", 0f},
        {"oil", 5f},
        {"coal", 0f},
        {"nothing", 995f}
    };

    // desert

    static Dictionary<string, float> desert = new Dictionary<string, float>() {
        {"iron", 0f},
        {"gold", 0f},
        {"oil", 10f},
        {"coal", 0f},
        {"nothing", 990f}
    };

    // swamp

    static Dictionary<string, float> swamp = new Dictionary<string, float>() {
        {"iron", 3f},
        {"gold", 9f},
        {"oil", 0f},
        {"coal", 10f},
        {"nothing", 987f}
    };

    // grass_land

    static Dictionary<string, float> grass_land = new Dictionary<string, float>() {
        {"iron", 10f},
        {"gold", 2f},
        {"oil", 0f},
        {"coal", 5f},
        {"nothing", 983f}
    };

    // rain_forest

    static Dictionary<string, float> rain_forest = new Dictionary<string, float>() {
        {"iron", 5f},
        {"gold", 10f},
        {"oil", 2f},
        {"coal", 3f},
        {"nothing", 980f}
    };

    // this looks evil
    Dictionary<string, Dictionary<string, float>> ResourceSpawnTable = new Dictionary<string, Dictionary<string, float>>() {
        {"tundra", tundra},
        {"water", water},
        {"swamp", swamp},
        {"grass_land", grass_land},
        {"rain_forest", rain_forest},
        {"desert", desert}
    };
    // love it!

    public string SpawnResource(string biomename) {
        try
        {
            return Util.RandomSelectChance<string>(ResourceSpawnTable[biomename]);
        }
        catch {
            Debug.Log("KeyNotFound: " + biomename);
            return null;
        }
    }

    public void ShowResource(MapTile tile) {
        Debug.Log("Runs!");
        if (tile.Data.ResourceType == "nothing" || tile.Data.ResourcesLeft <= 0) { return; }

        GameObject resource = null;

        switch (tile.Data.ResourceType) {
            case "iron":
                resource = Instantiate(ironPrefab);
                break;
            case "gold":
                resource = Instantiate(goldPrefab);
                break;
            case "coal":
                resource = Instantiate(coalPrefab);
                break;
            case "oil":
                resource = Instantiate(oilPrefab);
                break;
            default:
                return;
        }

        resource.transform.SetParent(tile.gameObject.transform);
        resource.transform.localPosition = new Vector3( 0, 0, resource.transform.position.z);
        
    }
}
