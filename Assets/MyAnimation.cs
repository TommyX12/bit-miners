using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyAnimation : MyMono {
    public Sprite[] anim;
    public SpriteRenderer rendy;
    public bool on;

    public float picker;


    private void Start()
    {
        rendy = GetComponent<SpriteRenderer>();
    }

    public override void PausingFixedUpdate()
    {
        base.PausingFixedUpdate();

        if (on)
        {
            if (picker < 1)
            {
                picker += Time.deltaTime;
            }
        }
        else {
            if (picker > 0) {
                picker -= Time.deltaTime;
            }
        }

        if (picker >= 1)
        {
            rendy.sprite = anim[anim.Length - 1];
        }
        else if (picker <= 0)
        {
            rendy.sprite = anim[0];
        }
        else
        {
            rendy.sprite = anim[(int)(picker * anim.Length - 1)];
        }
    }



}
