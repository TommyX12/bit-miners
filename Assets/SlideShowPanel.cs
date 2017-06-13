using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideShowPanel : MonoBehaviour {

    public GameObject NextPanel;

    public void Next() {
        NextPanel.SetActive(true);
        gameObject.SetActive(false);
    }
}
