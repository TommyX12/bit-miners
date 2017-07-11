using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class SECursor: SEElement {
    
    private float t = 0.0f;
    
    private Image image;
    private Color color;
    
    protected float FlashingSpeed = 10.0f;
    
    protected override void OnAwake() {
        this.image = this.GetComponent<Image>();
        this.color = this.image.color;
    }
    
    protected override void OnStart() {
        
    }
    
    public void Reflash() {
        this.t = 0;
    }
    
    public override void NormalUpdate() {
        if (this.gameObject.active) {
            this.t += Time.deltaTime;
            this.color.a = Util.Flashing(t, 0.0f, 2.0f, this.FlashingSpeed);
            this.image.color = color;
        }
    }
    
}
