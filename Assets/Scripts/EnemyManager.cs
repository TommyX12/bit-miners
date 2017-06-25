using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MyMono {
    
    public static EnemyManager Current;
    public List<GameObject> enemySource;
    public Dictionary<string, GameObject> Enemies;

    void Awake() {
        Current = this;
        Enemies = new Dictionary<string, GameObject>();
        foreach (GameObject obj in enemySource) {
            Enemies.Add(obj.name, obj);
        }
    }
    
    void Start() {
        
    }
    
    public override void PausingUpdate() {
        
    }
    
    // No longer stub!
    public Unit GetPrefabFromString(string enemyName) {
        if (Enemies.ContainsKey(enemyName))
        {
            return Enemies[enemyName].GetComponent<Unit>();
        }
        else {
            return null;
        }
    }
    
}
