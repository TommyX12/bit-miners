using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class TimedSwitch : MyMono {

    public Sprite[] onSprite;
    public Sprite offSprite;
    public float maxtime;
    public SpriteRenderer rendy;
    public bool on;
    public BoxCollider2D boxy;

    float selector;


    private void Start()
    {
        rendy = GetComponent<SpriteRenderer>();
        boxy = GetComponent<BoxCollider2D>();
    }

    public override void PausingFixedUpdate()
    {
        base.PausingFixedUpdate();

        if (on == false)
        {
            rendy.sprite = offSprite;
        }
        else {
            selector -= Time.fixedDeltaTime;
            if (selector <= 0)
            {
                on = false;
            }
            else {
                rendy.sprite = onSprite[(int)((selector / maxtime) * onSprite.Length-1)];
            }
        }
    }

    public void SetOn() {
        selector = maxtime;
        on = true;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        SetOn();
    }

}
