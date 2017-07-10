using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntranceElevator : MyMono {
    public GameObject target;
    public GameObject passenger;

    private void Start()
    {
        GameObject go = passenger.gameObject.transform.root.gameObject;
        passenger = go;
        passenger.GetComponent<SpriteRenderer>().sortingLayerName = "BelowMap";
        passenger.layer = LayerMask.NameToLayer("Bullets");
    }

    public override void PausingFixedUpdate()
    {
        base.PausingFixedUpdate();

        passenger.transform.position = transform.position;

        if ((transform.position - target.transform.position).magnitude > 0.05f)
        {
            transform.transform.position += (target.transform.position - transform.position).normalized * 0.1f * Time.fixedDeltaTime;
        }
        else {
            transform.position = target.transform.position;
            passenger.GetComponent<SpriteRenderer>().sortingLayerName = "Default";
            passenger.layer = LayerMask.NameToLayer("Unit");
            enabled = false;
        }

    }

}
