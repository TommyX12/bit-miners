using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponComponent : UnitComponent {

    public float CooldownTime;
    protected float cooldownTimer;

    public bool gameObjectTarget = true;

    public GameObject Target;
    private Vector2 vectorTarget;

    public virtual void fire() {
        if (cooldownTimer > 0) {
            return;
        }
        cooldownTimer = CooldownTime;
    }

    public void AimAtPosition(Vector2 position) {
        vectorTarget = position;
        gameObjectTarget = false;
    }

    public void TrackObject(GameObject obj) {
        gameObjectTarget = true;
        Target = obj;
    }

    public override void PausingFixedUpdate()
    {
        base.PausingFixedUpdate();

        if (Target == null && gameObjectTarget) {
            return;
        }

        Vector2 dv = gameObjectTarget ? (Vector2)(Target.transform.position - transform.position) : vectorTarget - (Vector2)transform.position;
        float da = Vector2.SignedAngle(transform.up, dv);
        // 5 degrees off is okay
        if (da >= -5 && da <= 5)
        {

        }
        else
        {
            transform.Rotate(new Vector3(0, 0, (da / Mathf.Abs(da)) * 30 * Time.fixedDeltaTime));
        }
    }

}
