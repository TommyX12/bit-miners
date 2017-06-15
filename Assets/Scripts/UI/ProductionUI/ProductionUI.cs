using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProductionUI : MyMono {

    public static ProductionUI Current;

    public ProductionComponent Production;
    public ProductionTooltip tooltip;
    public List<Image> ProductionBuildImages;
    public List<Image> ProductionQueueImages;
    public GameObject ProgressBar;

    public float UpdateRate;
    private float timer;

    public void Start()
    {
        Current = this;
    }

    public override void PausingUpdate()
    {
        base.PausingUpdate();
        if (Production == null) {
            return;
        }
        if (timer <= 0) {
            timer = UpdateRate;
            Refresh();
        }

        timer -= Time.deltaTime;

    }

    public void SetProductionComponent(ProductionComponent production) {

        Production = production;

        for (int i = 0; i < 15; i++) {
            ProductionBuildImages[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < production.BuildPrefabs.Count; i++) {
            ProductionBuildImages[i].gameObject.SetActive(true);
        }

        Refresh();
    }

    public void Refresh()
    {
        for (int i = 0; i < 15 && i < Production.BuildPrefabs.Count; i++) {
            ProductionBuildImages[i].sprite = Production.BuildPrefabs[i].GetComponent<SpriteRenderer>().sprite;
        }

        for (int i = 0; i < Production.buildQueue.Count; i++) {
            ProductionQueueImages[i].sprite = Production.buildQueue[i].prefab.GetComponent<SpriteRenderer>().sprite;
        }

        for (int i = Production.buildQueue.Count; i < 7; i++)
        {
            ProductionQueueImages[i].sprite = null;
        }

        float progress;

        if (Production.buildQueue.Count >= 1)
        {
            progress = 1 - Production.buildQueue[0].time / Production.buildQueue[0].prefab.GetComponent<Unit>().BuildTime;

        }
        else {
            progress = 0;
        }

        RectTransform rt = ProgressBar.GetComponent<RectTransform>();
        rt.anchorMax = new Vector2(progress, 1);
        rt.anchorMin = new Vector2(0, 0);
        rt.sizeDelta = new Vector2(0, 0);
        rt.offsetMax = new Vector2(0, 0);
        rt.offsetMin = new Vector2(0, 0);

        // logic to update progress bar

    }

    public void PostToolTipData(int index) {
        if (index > Production.BuildPrefabs.Count) {
            return;
        }
        tooltip.PostInfo(Production.BuildPrefabs[index].GetComponent<Unit>());
    }

    public void Cancel(int index) {
        Production.Cancel(index);
    }

    public void Build(int index) {
        Production.Build(index);
    }

}
