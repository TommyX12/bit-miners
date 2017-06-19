using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildUI : MyMono {
    public static BuildUI Current;
    public ResizingButtonPanel ItemPanel;
    public List<GameObject> ActiveList;
    public GameObject SelectedItem;
    public Toggle selected;

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
    }

}
