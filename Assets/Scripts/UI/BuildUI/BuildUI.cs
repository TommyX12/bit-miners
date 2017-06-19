using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildUI : MyMono {
    public static BuildUI Current;

    public ResizingButtonPanel ItemPanel;
    [HideInInspector]
    public List<GameObject> ActiveList;

    public GameObject SelectedItem;
    [HideInInspector]
    public Toggle selected;

    public GameObject IndicatorObject;
    Building selectedItemInfo;

    public void Start()
    {
        Current = this;
        gameObject.SetActive(false);
    }

    public void Toggle() {
        if (gameObject.activeSelf)
        {
            Deactivate();
        }
        else {
            Activate();
        }
    }

    public void Activate() {
        gameObject.SetActive(true);
        ProductionUI.Current.gameObject.SetActive(false);
        Inventory.Current.gameObject.SetActive(false);
    }

    public void Deactivate() {
        gameObject.SetActive(false);
        Inventory.Current.gameObject.SetActive(true);
        IndicatorObject.SetActive(false);
        SelectedItem = null;
    }

    public override void PausingUpdate()
    {
        base.PausingUpdate();

        if (SelectedItem != null) {
            bool CanPlace = false;
            selectedItemInfo = SelectedItem.GetComponent<Building>();

            GridCoord coord = Map.Current.Grid.PointToCoord((Vector2)Util.CameraToWorld(Camera.main, Input.mousePosition, 0));

            IndicatorObject.transform.position = Map.Current.Grid.CoordToPoint(coord);
            IndicatorObject.transform.position += new Vector3(0.25f * selectedItemInfo.xSize/2, selectedItemInfo.ySize/2 * - 0.25f,0);

            CanPlace = check(coord, selectedItemInfo.xSize,selectedItemInfo.ySize);

            if (Input.GetMouseButtonDown(1)) {
                IndicatorObject.SetActive(false);
                SelectedItem = null;
                return;
            }

            if (Input.GetMouseButtonDown(0)) {
                if (CanPlace) {
                    place(coord, selectedItemInfo.xSize, selectedItemInfo.ySize);
                }
            }

            if (CanPlace)
            {
                IndicatorObject.GetComponent<SpriteRenderer>().color = Color.white;
            }
            else {
                IndicatorObject.GetComponent<SpriteRenderer>().color = Color.red;
            }

        }
    }

    private void place(GridCoord topleft, int xSize, int ySize) {

        int xorigin = topleft.x;
        int yorigin = topleft.y;

        for (int y = 0; y < ySize; y++)
        {
            for (int x = 0; x < xSize; x++)
            {
                Map.Current.Grid.Get(new GridCoord(xorigin + x, yorigin - y)).Occupied = true;
            }
        }

        GameObject spawned = GameObject.Instantiate(SelectedItem);
        spawned.transform.position = IndicatorObject.transform.position;
        spawned.GetComponent<Building>().GridRef = topleft;

        Unit info = spawned.GetComponent<Unit>();

        Dictionary<string, int> cost = new Dictionary<string, int>();

        for (int i = 0; i < info.ResourceTypes.Count; i++)
        {
            cost.Add(info.ResourceTypes[i], info.ResourceCosts[i]);
        }

        NewResourceManager.Remove(cost);
    }

    private bool check(GridCoord topleft, int xSize, int ySize) {

        int xorigin = topleft.x;
        int yorigin = topleft.y;
        
        for (int y = 0; y < ySize; y++) {
            for (int x = 0; x < xSize; x++) {
                if (Map.Current.Grid.Get(new GridCoord(xorigin + x, yorigin - y)).Occupied) {
                    return false;
                }
            }
        }

        return true;
    }
}
