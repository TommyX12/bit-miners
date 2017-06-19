using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UICapsule : MonoBehaviour {
    public int index;
    public Image image;

    public void Select() {
        BuildUI.Current.SelectedItem = BuildUI.Current.ActiveList[index]; // Null Checking here
    }
}
