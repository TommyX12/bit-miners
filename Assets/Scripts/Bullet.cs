using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Bullet : MyMono {
    public int damage;
    public float velocity;
    private int teamid = -1;

    public override void PausingFixedUpdate()
    {
        base.PausingFixedUpdate();
        transform.position += transform.up * velocity * Time.fixedDeltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Unit unit;
        if ((unit = collision.GetComponent<Unit>()).teamid != teamid) {
            unit.ApplyDamage(damage);
        }
    }
}
