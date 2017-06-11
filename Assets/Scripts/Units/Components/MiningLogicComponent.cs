using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MiningLogicComponent : UnitComponent {

    public CircleCollider2D Sensor;
    public MoveComponent mover;
    public float SensorRange;
    private RaycastHit2D[] hits = new RaycastHit2D[50];


    private void Start()
    {
        Sensor.radius = SensorRange;
    }


    public GameObject GetNearestResource(string type) {
        int hitcount = Sensor.Cast(Vector2.zero, hits);

        GameObject winner = null;

        for (int i = 0; i < hitcount && i < hits.Length; i++) {
            if (hits[i].collider.gameObject.CompareTag("Resource") && hits[i].collider.gameObject.GetComponent<Resource>().type == type) {
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

	public Vector2 GetNearestResourcePosition(string type) {
		GameObject returnval;
		if ((returnval = GetNearestResource(type)) != null)
		{
			return returnval.transform.position;
		} else {
			return unit.transform.position;
		}
	}

    public Jurassic.Library.ObjectInstance GetNearestResourcePositionScript(string type) {
		return UnitComponent.Vector2ToObject(this.GetNearestResourcePosition(type));
    }

    public Jurassic.Library.ObjectInstance GetNearestSiloPositionScript(string type)
    {
        return UnitComponent.Vector2ToObject(this.GetNearestSiloPosition(type));
    }

    public void GoToNearestResource(string type) {
        mover.SetVectorTarget(GetNearestResourcePosition(type));
    }

    public GameObject GetNearestSilo(string type) {
        return NewResourceManager.GetNearestSilo(transform.position, type);
    }

    public Vector2 GetNearestSiloPosition(string type) {
        GameObject returnval;
        if ((returnval = GetNearestSilo(type)) != null) {
            return returnval.transform.position;
        } else { return transform.position; }
    }

    public void GoToNearestSilo(string type) {
        mover.SetVectorTarget(GetNearestSiloPosition(type));
    }

	public override void Register(ScriptSystem scriptSystem) {
		scriptSystem.RegisterFunction("get_nearest_resource_position", new Func<string, Jurassic.Library.ObjectInstance>(this.GetNearestResourcePositionScript));

        scriptSystem.RegisterFunction("get_nearest_silo_position", new Func<string, Jurassic.Library.ObjectInstance>(this.GetNearestSiloPositionScript));

        scriptSystem.RegisterFunction("go_to_nearest_resource", new Action<string> (GoToNearestResource));

        scriptSystem.RegisterFunction("go_to_nearest_silo", new Action<string> (GoToNearestSilo));
	}

}
