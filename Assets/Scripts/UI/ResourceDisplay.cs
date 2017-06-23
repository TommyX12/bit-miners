using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceDisplay : MyMono {
    public static ResourceDisplay Current;
    public List<Text> resourceCosts;
    Dictionary<string, Text> resources; // Very sensitive to order of resourceCosts

    private void Awake()
    {
        Current = this;
    }

    private void Start()
    {
        resources = new Dictionary<string, Text>()
        {
            {"iron", resourceCosts[0]},
            {"gold", resourceCosts[1]},
            {"coal", resourceCosts[2]},
            {"oil", resourceCosts[3]},
            {"chip", resourceCosts[4]},
            {"steel", resourceCosts[5]},
            {"plastic", resourceCosts[6]},
            {"processor", resourceCosts[7]},
            {"titanium", resourceCosts[8]},
        };
    }

    public override void PausingFixedUpdate()
    {
        base.PausingFixedUpdate();


        foreach (string key in new List<string>(NewResourceManager.Stored.Keys)) {
            resources[key].text = NewResourceManager.Stored[key]+"/"+NewResourceManager.Capacity[key];
        }
    }

}
