using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ScrollingText : MonoBehaviour {
    public Text text;
    public string data;
    public bool running;
    public bool isDone;
    public int CharactersPerSecond;
    private int index = 0;
    private float timer = 0;

    public void LoadText(string text) {
        data = text;
        isDone = false;
    }

    public void Display() {
        running = true;
        index = 0;
    }

    public void fastforward()
    {
        text.text = data.Substring(0, data.Length);
        running = false;
        isDone = true;
    }

    private void Update()
    {
        if (!running) {
            return;
        }

        if (timer * CharactersPerSecond >= 1)
        {
            index += (int)(timer * CharactersPerSecond);

            if (index >= data.Length) {
                running = false;
                isDone = true;
            }

            timer = 0;
            text.text = data.Substring(0, index);
        }
        timer += Time.deltaTime;
    }
}
