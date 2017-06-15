using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyMono : MonoBehaviour {
    public static bool Paused = false;

	public void Update()
	{
		if (!Paused) {
			this.PausingUpdate();
		}
		this.NormalUpdate();
	}

	public void FixedUpdate()
	{
		if (!Paused) {
			this.PausingFixedUpdate();
		}
		this.NormalFixedUpdate();
	}

    public void LateUpdate()
    {
		if (!Paused) {
			this.PausingLateUpdate();
		}
		this.NormalLateUpdate();
    }

	public virtual void PausingUpdate() {
		
	}
	
	public virtual void NormalUpdate() {
		
	}
	
    public virtual void PausingFixedUpdate() {
		
    }
	
	public virtual void NormalFixedUpdate() {
		
	}
	
	public virtual void PausingLateUpdate() {
		
	}
	
	public virtual void NormalLateUpdate() {
		
	}
	
}
