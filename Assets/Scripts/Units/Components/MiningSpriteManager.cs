using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class MiningSpriteManager : MyMono {

    public float UpdatePerXSeconds;
    public MiningComponent miner;
    public Sprite[] sprites;
    public SpriteRenderer rendy;

    private void Start()
    {
        rendy = GetComponent<SpriteRenderer>();
    }

    public void Refresh()
    {
        float seed = (float)miner.GetCurrentCapacity() / (float)miner.GetMaxCapacity();
        int sprite = (int)Mathf.Floor(seed * (sprites.Length - 1));
        rendy.sprite = sprites[sprite];
    }

}
