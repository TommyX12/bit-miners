using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SEElement : MyMono {
    
    private Vector2 position = new Vector2(0.0f, 0.0f);
    private Vector2 size = new Vector2(80.0f, 20.0f);
    private RectTransform rectTransform;
    
    void Awake() {
        this.rectTransform = this.GetComponent<RectTransform>();
        Util.TopLeftUIRectTransform(this.rectTransform);
        
        this.OnAwake();
    }
    
    void Start() {
        this.UpdatePosition();
        
        this.OnStart();
    }
    
    protected virtual void OnAwake() {
        
    }
    
    protected virtual void OnStart() {
        
    }
    
    public void SetPositionAndSize(Vector2 position, Vector2 size) {
        this.position = position;
        this.size = size;
        this.UpdatePosition();
    }
    
    public void SetWidth(float width) {
        this.size.x = width;
        this.UpdatePosition();
    }
    
    public void SetHeight(float height) {
        this.size.y = height;
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
    
    protected void UpdatePosition() {
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
