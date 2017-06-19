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
            BuildUI.Current.SelectedItem = BuildUI.Current.ActiveList[index]; // Null Checking here
            BuildUI.Current.IndicatorObject.GetComponent<SpriteRenderer>().sprite = BuildUI.Current.SelectedItem.GetComponent<SpriteRenderer>().sprite;
        }
    }
}
