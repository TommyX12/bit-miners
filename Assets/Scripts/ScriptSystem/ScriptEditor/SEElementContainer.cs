using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SEElementContainer : MyMono {
    
    [HideInInspector]
    public ScrollRect ScrollRect;
    
    [HideInInspector]
    public RectTransform Container;
    
    private Vector2 size = new Vector2(0.0f, 0.0f);
    
    public SEElementContainer() {
        
    }
    
    void Awake() {
        Util.SafeGetComponent<Mask>(this.gameObject);
        
        this.Container = Util.MakeEmptyUIContainer((RectTransform)this.transform).GetComponent<RectTransform>();
        this.Container.SetAsFirstSibling();
        this.Container.anchorMin = new Vector2(0.0f, 1.0f);
        this.Container.anchorMax = new Vector2(0.0f, 1.0f);
        this.Container.pivot     = new Vector2(0.0f, 1.0f);
        
        this.ScrollRect = Util.SafeGetComponent<ScrollRect>(this.gameObject);
        this.ScrollRect.content = this.Container;
        this.ScrollRect.viewport = (RectTransform)this.transform;
        this.ScrollRect.movementType = ScrollRect.MovementType.Clamped;
    }
    
    void Start() {
        
    }
    
    public void AddElement(SEElement element) {
        element.transform.SetParent(this.Container.transform, false);
    }
    
    public Vector2 GetSize() {
        return this.size;
    }
    
    public void SetSize(Vector2 size) {
        this.size = size;
        this.UpdatePosition();
    }
    
    private void UpdatePosition() {
        this.Container.offsetMin = new Vector2(
            0.0f, -this.size.y
        );
        this.Container.offsetMax = new Vector2(
            this.size.x, 0.0f
        );
    }
    
    public override void PausingUpdate() {
        
    }
    
}
