using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueDisplay : MonoBehaviour {
    public Text text;
    public Image portrait;


    public void Show(string text) {
        this.text.text = text;
    }
}
