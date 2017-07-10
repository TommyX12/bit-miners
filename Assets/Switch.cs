using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Switch : MyMono {
    public Sprite[] OnSprites;
    public Sprite OffSprite;
    public bool on = false;
    public float AnimationTime;
    SpriteRenderer rendy;
    float select;

    private void Start()
    {
        rendy = GetComponent<SpriteRenderer>();
    }


    public override void PausingFixedUpdate()
    {
        if (!on)
        {
            rendy.sprite = OffSprite;
        }
        else {
            select += Time.fixedDeltaTime;
            if (select >= AnimationTime) {
                select -= AnimationTime;
            }

            rendy.sprite = OnSprites[(int)((select / AnimationTime) * OnSprites.Length)];

        }
    }

    public void TurnOn() {
        select = 0;
        on = true;
    }

    public void TurnOff() {
        select = 0;
        on = false;
    }
}
