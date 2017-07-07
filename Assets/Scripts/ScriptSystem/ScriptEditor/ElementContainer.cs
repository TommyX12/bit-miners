using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ElementContainer : MyMono {
    
    [HideInInspector]
    public ScrollRect ScrollRect;
    
    public ElementContainer() {
        
    }
    
    void Awake() {
        this.gameObject.AddComponent<Mask>();
        
        
        
        this.ScrollRect = this.gameObject.AddComponent<ScrollRect>();
        this.ScrollRect.movementType = ScrollRect.MovementType.Clamped;
    }
    
    void Start() {
        
    }
    
    public override void PausingUpdate() {
        
    }
    
}
