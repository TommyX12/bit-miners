using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// For drones and AI units. Player Move Component will be different
public class MoveComponent : UnitComponent {
    public float speed;
    public float turnRate;
    private bool gameObjectTarget = true;
    public bool MoveToTarget = false;

    public GameObject gameObjectMoveTarget;
    private Vector2 vectorTarget;

    private void Start()
    {
        
    }

    public void SetGameObjectTarget(GameObject target) {
        MoveToTarget = target;
        gameObjectTarget = true;
    }

    public void SetVectorTarget(Vector2 target) {
        vectorTarget = target;
        gameObjectTarget = false;
    }

    public override void PausingFixedUpdate()
    {
        base.PausingFixedUpdate();
        if (MoveToTarget) {
            Vector2 dv = gameObjectTarget ? (Vector2) (gameObjectMoveTarget.transform.position - transform.position) : vectorTarget - (Vector2) transform.position;
            if (dv.magnitude <= 0.1f)
            {
                // close enough
            }
            else {
                transform.position += (Vector3) dv.normalized * speed * Time.fixedDeltaTime;
            }
        }
    }
}
