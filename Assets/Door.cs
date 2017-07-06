using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Door : UnitComponent {

    public Sprite[] animation;
    public Sprite DoorLockedSprite;
    public bool locked;
    public SpriteRenderer rendy;
    public Collider2D circlecollider;
    public BoxCollider2D boxcollider;
    public bool proximity;
    public float picker = 0;
    int count = 0;

    private void Start()
    {
        if (rendy == null)
        {
            rendy = GetComponent<SpriteRenderer>();
        }
    }

    public override void PausingUpdate()
    {
        base.PausingUpdate();

        if (count >= 1)
        {
            proximity = true;
        }
        else {
            proximity = false;
        }

        if (locked)
        {
            if (picker > 0)
            {
                picker -= Time.deltaTime;
            }
        }
        else {
            if (proximity)
            {
                if (picker < 1)
                {
                    picker += Time.deltaTime;
                }
            }
            else
            {
                if (picker > 0)
                {
                    picker -= Time.deltaTime;
                }
            }
        }

        if (picker <= 0)
        {
            if (locked)
            {
                rendy.sprite = DoorLockedSprite;
            }
            else {
                rendy.sprite = animation[0];
            }
        }
        else if (picker >= 1)
        {
            rendy.sprite = animation[animation.Length - 1];
            boxcollider.enabled = false;
        }
        else
        {
            rendy.sprite = animation[(int)((picker * animation.Length) - 1)];
            boxcollider.enabled = true;
        }
    }

    public void Lock() {
        locked = true;
    }

    public void Unlock() {
        locked = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        count++;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        count--;
    }

    public override void Register(ScriptSystem scriptSystem)
    {

        scriptSystem.RegisterFunction("lock", new Action(Lock));
        scriptSystem.RegisterFunction("unlock", new Action(Unlock));

    }
}
