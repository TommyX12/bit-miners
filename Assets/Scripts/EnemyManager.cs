using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MyMono {
    
    public static EnemyManager Current;
    
    void Awake() {
        Current = this;
    }
    
    void Start() {
        
    }
    
    public override void PausingUpdate() {
        
    }
    
    // stub
    public Unit GetPrefabFromString(string enemyName) {
        return null;
    }
    
}
