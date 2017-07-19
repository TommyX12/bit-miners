using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SEElement : MyMono {
    
    private Vector2 position = new Vector2(0.0f, 0.0f);
    private Vector2 size = new Vector2(80.0f, 20.0f);
    private RectTransform rectTransform;
    
    public SEElementDef Definition = null;
    
    [HideInInspector]
    public SEElementContainer Container = null;
    
    [HideInInspector]
    public int cachedRow    = 0;
    [HideInInspector]
    public int cachedColumn = 0;
    protected bool active   = false;
    
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
    
    public virtual void SetActive(bool active) {
        this.active = active;
    }
    
    public virtual void Remove() {
        Destroy(this.gameObject);
    }
    
    protected void UpdatePosition() {
        Vector2 size = this.size;
        if (this.Definition != null && this.Definition.ExtendSize) {
            size += new Vector2(4.0f, 0.0f);
        }
        this.rectTransform.offsetMin = new Vector2(
            this.position.x, -(this.position.y + size.y)
        );
        this.rectTransform.offsetMax = new Vector2(
            this.position.x + size.x, -this.position.y
        );
    }
    
    public void SetCache(int row, int column) {
        this.cachedRow    = row;
        this.cachedColumn = column;
    }
    
    public override void NormalUpdate() {
        
    }
    
    public virtual void SetupDefinition() {
        
    }
    
    public virtual void BeforeSave() {
        
    }
    
    public virtual void Init() {
        
    }
    
}
