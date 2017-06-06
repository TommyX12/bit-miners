using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MiningLogicComponent : UnitComponent {

    public CircleCollider2D Sensor;
    public MoveComponent mover;
    public float SensorRange;
    private RaycastHit2D[] hits = new RaycastHit2D[20];


    private void Start()
    {
        Sensor.radius = SensorRange;
    }


    public GameObject GetNearestResource() {
        int hitcount = Sensor.Cast(Vector2.zero, hits);

        GameObject winner = null;

        for (int i = 0; i < hitcount && i < hits.Length; i++) {
            if (hits[i].collider.gameObject.CompareTag("Resource")) {
                if (winner == null)
                {
                    winner = hits[i].collider.gameObject;
                }
                else {
                    if (((Vector2)(transform.position - winner.transform.position)).magnitude >
                        ((Vector2)(transform.position - hits[i].collider.gameObject.transform.position)).magnitude) {
                        winner = hits[i].collider.gameObject;
                    }
                }
            }
        }
        return winner;
    }

	public Vector2 GetNearestResourcePosition() {
		GameObject returnval;
		if ((returnval = GetNearestResource()) != null)
		{
			return returnval.transform.position;
		} else {
			return unit.transform.position;
		}
	}

    public Jurassic.Library.ObjectInstance GetNearestResourcePositionScript() {
		return UnitComponent.Vector2ToObject(this.GetNearestResourcePosition());
    }

    public void GoToNearestResource() {
        mover.SetVectorTarget(GetNearestResourcePosition());
    }

    public GameObject GetNearestSilo() {
        return ResourceManager.GetNearestSilo(transform.position);
    }

    public Vector2 GetNearestSiloPosition() {
        GameObject returnval;
        if ((returnval = GetNearestSilo()) != null) {
            return returnval.transform.position;
        } else { return transform.position; }
    }

    public void GoToNearestSilo() {
        mover.SetVectorTarget(GetNearestSiloPosition());
    }
	
	public override void Register(ScriptSystem scriptSystem) {
		scriptSystem.RegisterFunction("get_nearest_resource_position", new Func<Jurassic.Library.ObjectInstance>(this.GetNearestResourcePositionScript));
	}

}
