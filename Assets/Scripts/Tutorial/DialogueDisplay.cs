using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueDisplay : MonoBehaviour {
    public Text text;
    public float displayTimer = 3;
    float timer = 0;

    public void Show(string text) {
        this.text.text = text;
        timer = displayTimer;
        gameObject.SetActive(true);
    }

    public void Update()
    {
        timer -= Time.deltaTime;

        if (timer < 0) {
            gameObject.SetActive(false);
        }
    }

}
