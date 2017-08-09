using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveComponent : UnitComponent {
    
    public static PlayerMoveComponent Current;

    public float speed;
    
    void Awake() {
        Current = this;
    }

    public override void PausingFixedUpdate()
    {
        base.PausingFixedUpdate();
        int dx = 0;
        int dy = 0;
        if (Input.GetKey(KeyCode.W)) {
            dy++;
        }
        if (Input.GetKey(KeyCode.S)) {
            dy--;
        }
        if (Input.GetKey(KeyCode.A))
        {
            dx--;
        }
        if (Input.GetKey(KeyCode.D))
        {
            dx++;
        }

        transform.position += new Vector3(dx, dy) * Time.fixedDeltaTime; 
    }
}
