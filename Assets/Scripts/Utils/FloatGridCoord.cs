using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public struct FloatGridCoord {
	public float x;
	public float y;
	
	public FloatGridCoord(float x = 0.0f, float y = 0.0f) {
		this.x = x;
		this.y = y;
	}
	
	public GridCoord Round() {
		return new GridCoord((int)Math.Round(this.x), (int)Math.Round(this.y));
	}
}
