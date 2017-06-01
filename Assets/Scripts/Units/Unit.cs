using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MyMono {
    

    public int MaxHP;
    int currentHP;

    public int GetMaxHP() {
        return MaxHP;
    }

    public int GetCurrentHP() {
        return currentHP;
    }

    public void ApplyDamage(int damage) {
        currentHP -= damage;
    }

    public List<UnitComponent> components;

    // Use this for initialization

    void Start() {
    }

    // Update is called once per frame
    void Update() {
    }

    public UnitComponent getUnitcomponentOfType<T>() {
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
        }
    }

}
