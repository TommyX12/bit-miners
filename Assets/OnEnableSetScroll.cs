using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnEnableSetScroll : MonoBehaviour {
    public Scrollbar sc;
    public float value;
    private void OnEnable()
    {
        sc.value = value;
    }
}
