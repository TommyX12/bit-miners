using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(GridObject))]
public class Unit : MonoBehaviour {
    

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


}
