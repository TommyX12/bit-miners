using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 4 types of basic resource.
// iron - gold - oil - coal
// 3 types of secondary resource
// chip - steel - plastic
// 2 types of tertiary resource
// processor - titanium


public class Resource {
	
	public string SpriteName;
	public int MaxResourceValue;
	public int ResourceValue;
	public int ResourcePerCollection;
	public float CollectionTime;
	public string type;
	
	public MapData MapData;
	
	public Resource(MapData mapData = null) {
		this.MapData = mapData;
	}

    private static Dictionary<string, Resource> resourceDict = new Dictionary<string, Resource>() {
        {"iron", new Resource(){
            SpriteName = "iron",
            MaxResourceValue = 500,
            ResourceValue = 500,
            ResourcePerCollection = 5,
            CollectionTime = 3,
            type = "iron",
        }},
        {"gold", new Resource(){
            SpriteName = "gold",
            MaxResourceValue = 500,
            ResourceValue = 500,
            ResourcePerCollection = 5,
            CollectionTime = 3,
            type = "gold",
        }},
        {"oil", new Resource(){
            SpriteName = "oil",
            MaxResourceValue = 500,
            ResourceValue = 500,
            ResourcePerCollection = 5,
            CollectionTime = 3,
            type = "oil",
        }},
        {"coal", new Resource(){
            SpriteName = "coal",
            MaxResourceValue = 500,
            ResourceValue = 500,
            ResourcePerCollection = 5,
            CollectionTime = 3,
            type = "coal",
		}},
	};
	
	public static Resource Construct(string name, MapData mapData) {
		Resource result = (Resource)resourceDict[name].MemberwiseClone();
		result.MapData = mapData;
		return result;
	}
		
	public int GetResource() {
		int value = ResourcePerCollection;
		ResourceValue -= ResourcePerCollection;
		if (ResourceValue <= 0) {
			value += ResourceValue;
			this.MapData.RemoveResource();
		}
		return value;
	}
	
}
