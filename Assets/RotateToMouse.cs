using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateToMouse : MyMono {

    public override void PausingFixedUpdate()
    {
        Vector2 dv = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;

        gameObject.transform.rotation = Quaternion.LookRotation(Vector3.forward, dv);
    }

}
