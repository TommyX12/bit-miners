using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProductionUI : MyMono {

    public ProductionComponent Production;
    public List<Image> ProductionBuildImages;
    public List<Image> ProductionQueueImages;
    public GameObject ProgressBar;

    public float UpdateRate;
    private float timer;

    public override void PausingUpdate()
    {
        base.PausingUpdate();

        if (timer <= 0) {
            timer = UpdateRate;
            Refresh();
        }

        timer -= Time.deltaTime;

    }

    public void Refresh()
    {
        for (int i = 0; i < 4 && i < Production.BuildPrefabs.Count; i++) {
            ProductionBuildImages[i].sprite = Production.BuildPrefabs[i].GetComponent<SpriteRenderer>().sprite;
        }

        for (int i = 0; i < Production.buildQueue.Count; i++) {
            ProductionQueueImages[i].sprite = Production.buildQueue[i].prefab.GetComponent<SpriteRenderer>().sprite;
        }

        for (int i = Production.buildQueue.Count; i < 7; i++)
        {
            ProductionQueueImages[i].sprite = null;
        }


        // logic to update progress bar

    }

}
