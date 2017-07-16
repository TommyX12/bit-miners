using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Switch : SwitchBase {
    public Sprite[] OnSprites;
    public Sprite OffSprite;
    public float AnimationTime;
    public Collider2D colly;
    public bool oneShot;
    SpriteRenderer rendy;
    float select;
    int count = 0;

    private void Start()
    {
        rendy = GetComponent<SpriteRenderer>();
        colly = GetComponent<Collider2D>();
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        count++;
        if (count > 0) {
            TurnOn();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (oneShot) {
            return;
        }

        count--;
        if (count > 0)
        {

        }
        else {
            TurnOff();
        }
    }
}