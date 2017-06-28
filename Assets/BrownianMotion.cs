using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrownianMotion : MonoBehaviour {

    public float directionChangeInterval = 1;
    public float speed;
    float timer = 0;
    private Vector2 move;
    private void Start()
    {
        timer = directionChangeInterval;

        move = new Vector2(Random.Range(-1, 1), Random.Range(-1, 1));

        move.Normalize();
    }

    private void FixedUpdate()
    {
        transform.position += (Vector3)move / (1f / speed);
        timer -= Time.fixedDeltaTime;
        if (timer <= 0) {
            timer = directionChangeInterval;
            move = new Vector2(Random.Range(-1, 1), Random.Range(-1, 1));
            move.Normalize();
        }
    }

}