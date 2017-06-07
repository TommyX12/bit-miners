using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitComponent : MyMono, IScriptSystemAPI {
    public Unit unit;
	
	public static Jurassic.Library.ObjectInstance Vector2ToObject(Vector2 v) {
		Jurassic.Library.ObjectInstance obj = ScriptSystem.Current.ConstructObject();
		
		obj["x"] = (double)v.x;
		obj["y"] = (double)v.y;
		
		return obj;
	}

	public virtual void Register(ScriptSystem scriptSystem) {
		
	}
	
	public virtual void PreRun(ScriptSystem scriptSystem) {
		
	}
	
	public virtual void PostRun(ScriptSystem scriptSystem) {
		
	}
}
