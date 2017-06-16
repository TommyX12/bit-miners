using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


// For drones and AI units. Player Move Component will be different
public class MoveComponent : UnitComponent {
	public float speed;
	public float turnRate;
	private bool gameObjectTarget = true;
	public bool MoveToTarget = false;
	public bool IsMoveDirection = false;

	public GameObject gameObjectMoveTarget;
	private Vector2 vectorTarget;

	public void SetGameObjectTarget(GameObject target) {
		MoveToTarget = target;
		gameObjectTarget = true;
		MoveToTarget = true;
		IsMoveDirection = false;
	}

	public void SetVectorTarget(Vector2 target) {
		vectorTarget = target;
		gameObjectTarget = false;
		MoveToTarget = true;
		IsMoveDirection = false;

	}

	public void SetXYTarget(double x, double y) {
		vectorTarget = new Vector2((float)x, (float)y);
		gameObjectTarget = false;
		MoveToTarget = true;
		IsMoveDirection = false;
	}

	public void MoveDirection(double x, double y) {
		vectorTarget = (Vector2)transform.position + new Vector2((float)x, (float)y);
		gameObjectTarget = false;
		IsMoveDirection = true;
		MoveToTarget = true;
	}

	public override void PausingFixedUpdate()
	{
		base.PausingFixedUpdate();
		if (MoveToTarget) {
			Vector2 dv = gameObjectTarget ? (Vector2) (gameObjectMoveTarget.transform.position - transform.position) : vectorTarget - (Vector2) transform.position;
			float da = Vector2.SignedAngle(transform.up, dv);

			if (dv.magnitude <= 0.25f)
			{
				// close enough
			}
			else {
				transform.position += (Vector3) dv.normalized * speed * Time.fixedDeltaTime;
			}

			// 5 degrees off is okay
			if (da >= -5 && da <= 5)
			{

			}
			else {
				transform.Rotate(new Vector3(0, 0, (da / Mathf.Abs(da)) * turnRate * Time.fixedDeltaTime));
			}

			if (IsMoveDirection) {
				vectorTarget = (Vector2)transform.position + dv;
			}
		}
	}

	public void Stop() {
		MoveToTarget = false;
	}

	public void MoveToWaypoint(string name) {
		WaypointManager.Waypoint waypoint = WaypointManager.Current.GetWaypoint(name);
		if (waypoint != null) {
			SetVectorTarget(waypoint.Pos);
		}
		else {
			Stop();
		}
	}

	public override void Register(ScriptSystem scriptSystem) {
		scriptSystem.RegisterFunction("move_to", new Action<double, double>(this.SetXYTarget));
		scriptSystem.RegisterFunction("move_in_direction", new Action<double, double>(this.MoveDirection));
		scriptSystem.RegisterFunction("move_to_waypoint", new Action<string>(this.MoveToWaypoint));
		scriptSystem.RegisterFunction("stop", new Action(this.Stop));
	}
}
