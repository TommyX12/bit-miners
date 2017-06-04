using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MyMono {

    public float BuildTime;
    public int BuildCost;
    public int MaxHP;
    public int teamid;
    int currentHP;
 

    private void Start()
    {
        RegisterComponents();
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

    public List<UnitComponent> components;

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
        }
    }


}
