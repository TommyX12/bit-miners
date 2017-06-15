using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowScript : MyMono {
    public GameObject target;
	
	public float Smoothing = 0.9f;
	
	public float ZOffsetMin = -5.0f;
	public float ZOffsetMax = -1.0f;
	public float ZOffset = -2.0f;
	public float ZOffsetScrollSpeed = 0.25f;
	
	private Vector3 destination = new Vector3();
	
	void Awake() {
		
	}

	void Start() {
		
	}

    public override void PausingFixedUpdate() {
		float scroll = Input.GetAxis("Mouse ScrollWheel");
		if (scroll > 0) {
			this.ZOffset += this.ZOffsetScrollSpeed;
		}
		else if (scroll < 0) {
			this.ZOffset -= this.ZOffsetScrollSpeed;
		}
		
		this.ZOffset = Util.Clamp(this.ZOffset, this.ZOffsetMin, this.ZOffsetMax);
		
		this.destination = this.target.transform.position + new Vector3(0, 0, this.ZOffset);
        this.gameObject.transform.position = Vector3.Lerp(this.gameObject.transform.position, this.destination, 1.0f - this.Smoothing);
    }
}
