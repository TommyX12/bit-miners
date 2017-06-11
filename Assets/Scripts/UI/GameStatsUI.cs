using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameStatsUI : MyMono {

    public Text ResourceText;
    public float UpdateRate;
    public float timer;

    public override void NormalFixedUpdate()
    {
        base.NormalFixedUpdate();
        if (timer <= 0) {
            timer = UpdateRate;
            Refresh();
        }

        timer -= Time.deltaTime;

    }

    public void Refresh() {
        string str = "Resources:";
        foreach (string key in new List<string>(NewResourceManager.Capacity.Keys)) {
            str += " " + key + "-" + NewResourceManager.Stored[key] + "/" + NewResourceManager.Capacity[key];
        }
        ResourceText.text = str;
    }

}
