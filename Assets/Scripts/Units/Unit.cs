using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Unit : MyMono, IScriptSystemAPI {

	public float BuildTime;

    public List<string> ResourceTypes;
    public List<int> ResourceCosts;

    public int MaxHP;

	public int teamid;
	public string description;
	int currentHP;
 
	public List<UnitComponent> components;
	
	public Button ScriptButtonObject;
	
	private float timer = 0.0f;
	
	private void StartEditor() {
		GameManager.Current.ScriptEditorObject.StartEdit(this.ScriptSystemObject);
	}
	
	public ScriptSystem ScriptSystemObject {
		get; set;
	}
	
	private void Awake() {
		this.ScriptSystemObject = new ScriptSystem();
		this.RegisterComponents();
		this.RegisterScriptSystem();
		
		if (this.ScriptButtonObject != null) this.ScriptButtonObject.onClick.AddListener(this.StartEditor);
	}

	private void Start()
	{
		this.ScriptSystemObject.Script = @"";
		this.ScriptSystemObject.Start();
		
		currentHP = MaxHP;
	}
	
	public override void PausingUpdate() {
		this.ScriptUpdate();
	}
	
	private void ScriptUpdate() {
		this.ScriptSystemObject.DispatchEvent("update", (double)Time.deltaTime);
		while (this.timer >= 1.0f) {
			this.ScriptSystemObject.DispatchEvent("update_per_second");
			this.timer -= 1.0f;
		}
		
		this.timer += Time.deltaTime;
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
        UpdateHealthBar();
	}

    public void UpdateHealthBar() {
        HealthBar bar;

        if ((bar = (HealthBar) GetUnitComponent<HealthBar>())) {
            bar.Refresh(((float)currentHP / (float) MaxHP));
        }
    }

	public UnitComponent GetUnitComponent<T>() {
		foreach (UnitComponent comp in components) {
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
		}
	}
	
	public void RegisterScriptSystem() {
		List<IScriptSystemAPI> apiList = new List<IScriptSystemAPI>();
		apiList.Add(this);
		foreach (var component in this.components) {
			apiList.Add(component);
		}
		
		this.ScriptSystemObject.SetAPIList(apiList);
	}

    public Jurassic.Library.ObjectInstance GetPositionScript()
    {
        return UnitComponent.Vector2ToObject(transform.position);
    }

    public void Register(ScriptSystem scriptSystem)
    {
        scriptSystem.RegisterFunction("get_position", new Func<Jurassic.Library.ObjectInstance>(GetPositionScript));
        scriptSystem.RegisterFunction("get_max_hp", new Func<int>(GetMaxHP));
        scriptSystem.RegisterFunction("get_current_hp", new Func<int>(GetCurrentHP));
		scriptSystem.RegisterEvent("update", new string[]{"time_passed"});
		scriptSystem.RegisterEvent("update_per_second", new string[]{});
    }
	
	public void PreRun(ScriptSystem scriptSystem) {
		
	}
	
	public void PostRun(ScriptSystem scriptSystem) {
		this.ScriptSystemObject.DispatchEvent("update", 0.0);
	}
}
