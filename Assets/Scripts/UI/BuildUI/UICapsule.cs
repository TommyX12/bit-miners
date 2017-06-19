using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UICapsule : MonoBehaviour {
    public int index;
    public Image image;

    public void Select() {
        if (BuildUI.Current.selected != null)
        {
            GameObject item = BuildUI.Current.ActiveList[index];
            Unit info = item.GetComponent<Unit>();
            Dictionary<string, int> cost = new Dictionary<string, int>();

            for (int i = 0; i < info.ResourceTypes.Count; i++)
            {
                cost.Add(info.ResourceTypes[i], info.ResourceCosts[i]);
            }

            if (NewResourceManager.HasEnough(cost))
            {
                BuildUI.Current.SelectedItem = BuildUI.Current.ActiveList[index]; // Null Checking here
                BuildUI.Current.IndicatorObject.GetComponent<SpriteRenderer>().sprite = BuildUI.Current.SelectedItem.GetComponent<SpriteRenderer>().sprite;
                BuildUI.Current.IndicatorObject.SetActive(true);
            }
        }
    }
}
