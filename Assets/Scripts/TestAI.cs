using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAI : MonoBehaviour {
    public CombatComponent combat;

    private void Start()
    {
        combat.SetEngagementDistance(1f);
        combat.StopHoldingPosition();
    }

    void Update () {
        if (!combat.isAttacking())
        {
            combat.AttackRandomEnemy();
        }
	}
}
