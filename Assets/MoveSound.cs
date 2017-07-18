using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveSound : MyMono {

    public float distanceThreshold = 0.02f;
    public AudioClip stepSound;
    float deltapos = 0;
    Vector2 previouspos;
    AudioSource player;

    private void Start()
    {
        previouspos = transform.position;
        player = GetComponent<AudioSource>();
    }

    public override void PausingFixedUpdate()
    {
        base.PausingFixedUpdate();

        Vector2 currentpos = transform.position;
        deltapos += (currentpos - previouspos).magnitude;
        if (deltapos > distanceThreshold)
        {
            player.clip = stepSound;
            player.Play();   
            deltapos = 0;
        }
        previouspos = currentpos;
    }
}
