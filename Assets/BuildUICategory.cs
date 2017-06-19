using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BuildUICategory : MyMono {
    public List<GameObject> items;
    public Toggle toggle;

    public void Select()
    {
        if (toggle.isOn)
        {
            BuildUI.Current.ItemPanel.gameObject.SetActive(true);
            BuildUI.Current.selected = toggle;                                                                                                                                                                  
            BuildUI.Current.ActiveList = items;
            BuildUI.Current.ItemPanel.Refresh(items);
            BuildUI.Current.ItemPanel.rt.localPosition = GetComponent<RectTransform>().localPosition;
        }
        else {
            if (BuildUI.Current.selected == toggle) {
                BuildUI.Current.ItemPanel.gameObject.SetActive(false);
            }
            return;
        }
    }
}
