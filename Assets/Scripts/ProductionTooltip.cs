using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProductionTooltip : MonoBehaviour {
    public Text title;
    public Text buildtime;
    public Text description;

    public List<Text> resourceCosts;
    Dictionary<string, Text> resourceCostsHelper; // Very sensitive to order of resourceCosts

    private void Start()
    {
        resourceCostsHelper = new Dictionary<string, Text>()
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
        gameObject.SetActive(false);
    }
    public void PostInfo(Unit unit) {
        title.text = unit.gameObject.name;

        for (int i = 0; i < resourceCosts.Count; i++) {
            resourceCosts[i].text = "0";
        }

        for (int i = 0; i < unit.ResourceTypes.Count; i++) {
            resourceCostsHelper[unit.ResourceTypes[i]].text = unit.ResourceCosts[i].ToString();
        }

        buildtime.text = unit.BuildTime.ToString();
        description.text = unit.description;
    }
}
