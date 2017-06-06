using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CombatComponent : UnitComponent {

    public List<WeaponComponent> Weapons;
    public MoveComponent Movement;
    public Collider2D SensorCollider;

    private GameObject enemy;
    private bool attacking;
    private bool attackingObject;
    private bool holdPosition;
    private RaycastHit2D[] hits = new RaycastHit2D[20];

    private float EngangementDistance;

    private Vector2 positionTarget;

    public override void PausingFixedUpdate()
    {
        base.PausingFixedUpdate();
        if (attacking) {
            if (enemy == null) {
                attacking = false;
                return;
            }
            if (isHoldingPosition())
            {

            }
            else {
                Vector2 targetPos = attackingObject ? (Vector2)enemy.transform.position : positionTarget;
                if (Mathf.Abs(((targetPos - (Vector2)transform.position)).magnitude) - EngangementDistance < 0.05)
                {
                        Movement.Stop();
                }
                else if (((targetPos - (Vector2)transform.position)).magnitude - EngangementDistance < 0.05)
                {
                    Movement.SetVectorTarget((Vector2)transform.position - (targetPos - (Vector2)transform.position));
                }
                else if (((targetPos - (Vector2)transform.position)).magnitude - EngangementDistance > 0.05)
                {
                    Movement.SetVectorTarget(targetPos);
                }

            }
            foreach (WeaponComponent weapon in Weapons)
            {
                weapon.fire();
            }
        }
    }

    public void AttackNearestEnemy() {
        attackingObject = true;
        if ((enemy = GetNearestEnemy())!= null) {
            attacking = true;
        }

        foreach (WeaponComponent weapon in Weapons)
        {
            weapon.TrackObject(enemy);
        }
    }

    public void AttackRandomEnemy() {
        attackingObject = true;
        if ((enemy = GetRandomEnemy()) != null)
        {
            attacking = true;
        }

        foreach (WeaponComponent weapon in Weapons)
        {
            weapon.TrackObject(enemy);
        }
    }

    public void StopAttacking() {
        attacking = false;
        attackingObject = true;
    }

    public GameObject GetNearestEnemy() {
        int hitcount = SensorCollider.Cast(Vector2.zero, hits);
        GameObject winner = null;
        for (int i = 0; i < hitcount && i < hits.Length; i++)
        {
            if (hits[i].collider.gameObject.GetComponent<Unit>().teamid != unit.teamid)
            {
                if (winner == null)
                {
                    winner = hits[i].collider.gameObject;
                }
                else
                {
                    if ((transform.position - winner.transform.position).magnitude > (transform.position - hits[i].collider.gameObject.transform.position).magnitude)
                    {
                        winner = hits[i].collider.gameObject;
                    }
                }
            }
        }
        if (winner != null)
        {
            return winner;
        }
        else {
            return null;
        }
    }

    public GameObject GetRandomEnemy() {
        int hitcount = SensorCollider.Cast(Vector2.zero, hits);
        for (int i = 0; i < hitcount && i < hits.Length; i++)
        {
            if (hits[i].collider.gameObject.GetComponent<Unit>().teamid != unit.teamid)
            {
                return hits[i].collider.gameObject;
            }
        }
        return null;
    }

    public void AttackPosition(Vector2 pos) {
        positionTarget = pos;
        attacking = true;
        attackingObject = false;

        foreach (WeaponComponent weapon in Weapons)
        {
            weapon.AimAtPosition(pos);
        }
    }

    public void HoldPosition() {
        holdPosition = true;
    }

    public void StopHoldingPosition() {
        holdPosition = false;
    }

    public bool isHoldingPosition() {
        return holdPosition;
    }

    public bool isAttacking() {
        return attacking;
    }

    public void SetEngagementDistance(float distance) {
        EngangementDistance = distance;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject != null && collision.gameObject == enemy) {
            attacking = false;
        }
    }

    public override void Register(ScriptSystem scriptSystem)
    {
        base.Register(scriptSystem);
    }

}
