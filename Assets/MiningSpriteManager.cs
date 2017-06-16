using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class MiningSpriteManager : MyMono {

    public float UpdatePerXSeconds;
    public MiningComponent miner;
    public Sprite[] sprites;
    public SpriteRenderer rendy;
    private float timer;

    private void Start()
    {
        rendy = GetComponent<SpriteRenderer>();
    }

    public override void PausingFixedUpdate()
    {
        if (timer <= 0)
        {
            float seed = (float)miner.GetCurrentCapacity() / (float)miner.GetMaxCapacity();
            int sprite = (int)Mathf.Floor(seed * (sprites.Length-1));
            Debug.Log(sprite);
            rendy.sprite = sprites[sprite];
            timer = UpdatePerXSeconds;
        }
        else {
            timer -= Time.fixedDeltaTime;
        }
    }

}
