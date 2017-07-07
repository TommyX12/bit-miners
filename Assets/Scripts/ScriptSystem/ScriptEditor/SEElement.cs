using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SEElement : MyMono {
    
    private Vector2 position = new Vector2(0.0f, 0.0f);
    private Vector2 size = new Vector2(80.0f, 20.0f);
    private RectTransform rectTransform;
    
    public SEElement() {
        
    }
    
    void Awake() {
        this.rectTransform = this.GetComponent<RectTransform>();
        this.rectTransform.anchorMin = new Vector2(0.0f, 1.0f);
        this.rectTransform.anchorMax = new Vector2(0.0f, 1.0f);
    }
    
    void Start() {
        this.UpdatePosition();
    }
    
    public void SetPositionAndSize(Vector2 position, Vector2 size) {
        this.position = position;
        this.size = size;
        this.UpdatePosition();
    }
    
    public Vector2 GetPosition() {
        return this.position;
    }
    
    public void SetPosition(Vector2 position) {
        this.position = position;
        this.UpdatePosition();
    }
    
    public Vector2 GetSize() {
        return this.size;
    }
    
    public void SetSize(Vector2 size) {
        this.size = size;
        this.UpdatePosition();
    }
    
    private void UpdatePosition() {
        this.rectTransform.offsetMin = new Vector2(
            this.position.x, -(this.position.y + this.size.y)
        );
        this.rectTransform.offsetMax = new Vector2(
            this.position.x + this.size.x, -this.position.y
        );
    }
    
    public override void PausingUpdate() {
        
    }
    
}
