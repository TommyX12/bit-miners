using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorScript : MyMono {



    GameObject passenger;
    public GameObject transition_panel;

    public override void PausingFixedUpdate()
    {
        base.PausingFixedUpdate();
        if (passenger != null)
        {
            transform.position += (Vector3) new Vector2(0,-0.2f)*Time.fixedDeltaTime;
            passenger.transform.position = transform.position;
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) {
            GameObject go = collision.gameObject.transform.root.gameObject;
            passenger = go;
            passenger.GetComponent<SpriteRenderer>().sortingLayerName = "BelowMap";
            passenger.layer = LayerMask.NameToLayer("Bullets");
            transition_panel.SetActive(true);
        }
    }
}
