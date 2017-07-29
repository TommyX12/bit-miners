using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MyUIAnimation : MonoBehaviour {

    public Sprite[] anim;
    public Sprite offSprite;
    public Image rendy;
    public bool on;
    public bool repeat;
    public float picker;
    public float animTime;

    private void Start()
    {
        rendy = GetComponent<Image>();
    }

    public void FixedUpdate()
    {
        if (on)
        {
            if (picker < animTime + 1)
            {
                picker += Time.deltaTime;
            }
        }
        else
        {
            if (picker > 0)
            {
                picker -= Time.deltaTime;
                rendy.sprite = offSprite;
                return;
            }
        }

        if (picker >= animTime + 1)
        {
            rendy.sprite = anim[anim.Length - 1];
            if (repeat)
            {
                picker = 0;
            }
        }
        else if (picker <= 0 )
        {
            rendy.sprite = anim[0];
        }
        else
        {
            rendy.sprite = anim[(int)(((float)picker/animTime) * anim.Length - 1)];
        }
    }
}
