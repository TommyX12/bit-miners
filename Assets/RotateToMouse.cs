using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateToMouse : MyMono {
	
	public float Smoothing = 0.9f;
	public float AngleOffset = 90.0f;

    public override void PausingFixedUpdate()
    {
        Vector2 target = Util.CameraToWorld(Camera.main, Input.mousePosition, this.gameObject.transform.position.z);
		float angle = Util.GetAngle(target, this.gameObject.transform.position, false) + this.AngleOffset;
		Vector3 eulerAngles = this.gameObject.transform.eulerAngles;
        eulerAngles.z += (1.0f - this.Smoothing) * Util.RotationDist(eulerAngles.z, angle, false);
		this.gameObject.transform.eulerAngles = eulerAngles;
    }

}
