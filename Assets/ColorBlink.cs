using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorBlink : MonoBehaviour {

    public List<Color> colors;
    public Color defaultColor;
    public float timePerColor;
    public Image image;
    public bool On = false;

    private float timer;
    private int index = 0;

    // Use this for initialization
    void Start () {
        if (image == null) {
            image = GetComponent<Image>();
        }
	}
	
	// Update is called once per frame
	void Update () {
        if (On)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                if (index == colors.Count-1)
                {
                    index = 0;
                    timer = timePerColor;
                }
                else
                {
                    index++;
                    timer = timePerColor;
                }
            }
            image.color = colors[index];
        }
        else {
            timer = timePerColor;
            index = 0;
            image.color = defaultColor;
        }
	}
}
