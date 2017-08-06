using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Flashing : MyMono {
    private float t = 0.0f;
    
    private Image image;
    private Color color;
    
    public float FlashingSpeed = 10.0f;
    
    public float FlashingMin = 0.0f;
    public float FlashingMax = 1.0f;
    
    void Awake() {
        this.image = this.GetComponent<Image>();
        this.color = this.image.color;
    }
    
    public override void NormalUpdate() {
        if (this.gameObject.active) {
            this.color.a = Util.Flashing(Time.time, this.FlashingMin, this.FlashingMax, this.FlashingSpeed);
            this.image.color = color;
        }
    }
}
