using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class SingleFireWeapon : WeaponComponent {

    public GameObject bulletPrefab;
    public GameObject FirePoint;

    public override void fire()
    {
        if (cooldownTimer > 0)
        {
            return;
        }
        cooldownTimer = CooldownTime;

        GameObject bullet = GameObject.Instantiate(bulletPrefab);
        bullet.transform.position = new Vector3(FirePoint.transform.position.x, FirePoint.transform.position.y, bulletPrefab.transform.position.z);
        bullet.transform.rotation = FirePoint.transform.rotation;
        bullet.GetComponent<Bullet>().setTeamId(unit.teamid);
    }

    public override void PausingFixedUpdate()
    {
        base.PausingFixedUpdate();
        if (cooldownTimer > 0) {
            cooldownTimer -= Time.fixedDeltaTime;
        }
    }

    public bool ReadyToFire() {
        return cooldownTimer <= 0;
    }

    public double GetCooldown() {
        return cooldownTimer;
    }

    public override void Register(ScriptSystem scriptSystem)
    {
        scriptSystem.RegisterFunction("ready_to_fire", new Func<bool>(ReadyToFire), true, false);
        scriptSystem.RegisterFunction("cooldown_left", new Func<double>(GetCooldown), true, false);
    }
}
