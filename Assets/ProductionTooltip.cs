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
        cost.text = "Cost: " + unit.BuildCost;
        buildtime.text = "Build Time: " + unit.BuildTime;
        description.text = unit.description;
    }
}
