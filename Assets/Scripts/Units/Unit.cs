using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Unit : MyMono, IScriptSystemAPI {

	public float BuildTime;
	public int BuildCost;
	public int MaxHP;
	public int teamid;
	public string description;
	int currentHP;
 
	public List<UnitComponent> components;
	
	public ScriptSystem ScriptSystemObject {
		get; set;
	}
	
	private void Awake() {
		this.ScriptSystemObject = new ScriptSystem();
		this.RegisterComponents();
	}

	private void Start()
	{
		this.ScriptSystemObject.Script = @"";
		this.ScriptSystemObject.Start();
		
		currentHP = MaxHP;
	}

	public int GetMaxHP() {
		return MaxHP;
	}

	public int GetCurrentHP() {
		return currentHP;
	}

	public void ApplyDamage(int damage) {
		currentHP -= damage;
		if (currentHP <= 0) {
			Destroy(gameObject);
		}
	}

	public UnitComponent GetUnitComponent<T>() {
		foreach (UnitComponent comp in components) {
			Debug.Log(comp.GetType() + " " +  typeof(T));
			if (comp.GetType() == typeof(T)) {
				return comp;
			}
		}
		return null;
	}

	public void RegisterComponents() {
		foreach (UnitComponent component in GetComponentsInChildren<UnitComponent>()) {
			component.unit = this;
			components.Add(component);
			
			this.ScriptSystemObject.APIList.Add(component);
		}
	}

    public Jurassic.Library.ObjectInstance GetPositionScript()
    {
        return UnitComponent.Vector2ToObject(transform.position);
    }

    public void Register(ScriptSystem scriptSystem)
    {
        scriptSystem.RegisterFunction("get_position", new   Func<Jurassic.Library.ObjectInstance>(GetPositionScript));
    }

    public void PostRegister(ScriptSystem scriptSystem)
    {
        
    }
}
