using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProductionTooltip : MonoBehaviour {
    public Text title;
    public Text cost;
    public Text buildtime;
    public Text description;

    public void PostInfo(Unit unit) {
        title.text = unit.gameObject.name;
        string str = "Cost:";
        for (int i = 0; i < unit.ResourceTypes.Count; i++) {
            str += " " + unit.ResourceTypes[i] + "|" +  unit.ResourceCosts[i];
        }
        cost.text = str;
        buildtime.text = "Build Time: " + unit.BuildTime;
        description.text = unit.description;
    }
}
