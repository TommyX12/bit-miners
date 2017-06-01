using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyMono : MonoBehaviour {
    public static bool Paused = false;

	public void Update()
	{
		if (!Paused) {
			PausingUpdate();
		}
		NormalUpdate();
	}

    public void FixedUpdate()
    {
		if (!Paused) {
			PausingFixedUpdate();
		}
		NormalFixedUpdate();
    }

	public virtual void PausingUpdate() {
		
	}
	
	public virtual void NormalUpdate() {
		
	}
	
    public virtual void PausingFixedUpdate() {
		
    }
	
	public virtual void NormalFixedUpdate() {
		
	}
}
