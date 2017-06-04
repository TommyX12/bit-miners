using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleFireWeapon : WeaponComponent {

    public GameObject bulletPrefab;
    public GameObject FirePoint;

    public override void fire()
    {
        base.fire();
        GameObject bullet = GameObject.Instantiate(bulletPrefab);
        bullet.transform.position = new Vector3(FirePoint.transform.position.x, FirePoint.transform.position.y, bulletPrefab.transform.position.z);
    }

    public override void PausingFixedUpdate()
    {
        base.PausingFixedUpdate();

        if (cooldownTimer > 0) {
            cooldownTimer -= Time.fixedDeltaTime;
        }
    }

}
