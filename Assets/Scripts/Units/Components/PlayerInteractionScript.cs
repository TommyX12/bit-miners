using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractionScript : UnitComponent {

    public Collider2D coll;

    public override void PausingUpdate()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            RaycastHit2D[] hits = new RaycastHit2D[20];
            int hitCount = coll.Cast(Vector2.zero, hits);
            for (int i = 0; i < hitCount; i++)
            {
                if (hits[i].collider.gameObject.GetComponent<IInteractable>() != null)
                {
                    hits[i].collider.gameObject.GetComponent<IInteractable>().Interact(unit.gameObject);
                }
            }
        }
    }
}
