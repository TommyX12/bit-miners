using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SEElementContainer : MyMono {
    
    [HideInInspector]
    public ScrollRect ScrollRect;
    
    [HideInInspector]
    public RectTransform Container;
    
    protected Vector2 size = new Vector2(0.0f, 0.0f);
    protected List<List<SEElement>> data = new List<List<SEElement>>();
    
    [HideInInspector]
    protected float ElementHeight = 22.5f;
    
    [HideInInspector]
    protected float VerticalSpacing = 3.0f;
    
    [HideInInspector]
    protected float HorizontalSpacing = 3.0f;
    
    void Awake() {
        Util.SafeGetComponent<Mask>(this.gameObject);
        
        this.Container = Util.MakeEmptyUIContainer((RectTransform)this.transform).GetComponent<RectTransform>();
        this.Container.SetAsFirstSibling();
        Util.TopLeftUIRectTransform(this.Container);
        this.Container.pivot     = new Vector2(0.0f, 1.0f);
        
        this.ScrollRect = Util.SafeGetComponent<ScrollRect>(this.gameObject);
        this.ScrollRect.content = this.Container;
        this.ScrollRect.viewport = (RectTransform)this.transform;
        this.ScrollRect.movementType = ScrollRect.MovementType.Clamped;
        
        EventTrigger trigger = Util.SafeGetComponent<EventTrigger>(this.gameObject);
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerDown;
        entry.callback.AddListener((data) => { OnPointerDown((PointerEventData)data); });
        trigger.triggers.Add(entry);
        
        this.OnAwake();
    }
    
    void Start() {
        
        this.OnStart();
        this.Redraw();
    }
    
    protected virtual void OnAwake() {
        
    }
    
    protected virtual void OnStart() {
        
    }
    
    public void AddElement(int row, int column, SEElement element) {
        element.transform.SetParent(this.Container.transform, false);
        element.SetHeight(this.ElementHeight);
        row = Util.Clamp(row, 0, this.data.Count);
        if (row == this.data.Count) {
            this.data.Add(new List<SEElement>());
        }
        column = Util.Clamp(column, 0, this.data[row].Count);
        this.data[row].Insert(column, element);
        this.Redraw();
    }
    
    public Vector2 GetSize() {
        return this.size;
    }
    
    public void SetSize(Vector2 size) {
        this.size = size;
        this.UpdatePosition();
    }
    
    public void Redraw() {
        Vector2 pos = new Vector2(0.0f, 0.0f);
        float rowHeight = this.ElementHeight + this.VerticalSpacing;
        foreach (var row in this.data) {
            pos.x = 0;
            foreach (var element in row) {
                element.SetPosition(pos);
                pos.x += element.GetSize().x + this.HorizontalSpacing;
            }
            pos.y += rowHeight;
        }
        
        this.OnRedraw();
    }
    
    protected virtual void OnRedraw() {
        
    }
    
    private void UpdatePosition() {
        this.Container.offsetMin = new Vector2(
            0.0f, -this.size.y
        );
        this.Container.offsetMax = new Vector2(
            this.size.x, 0.0f
        );
    }
    
    public override void NormalUpdate() {
        
    }
    
    private void OnPointerDown(PointerEventData data) {
        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(this.Container.transform as RectTransform, data.position, data.pressEventCamera, out pos);
        
        int row = (int)Math.Floor(-pos.y / (this.ElementHeight + this.VerticalSpacing));
        row = Util.Clamp(row, 0, this.data.Count);
        int column = 0;
        SEElement element = null;
        if (row < this.data.Count) {
            float accWidth = 0.0f;
            for (; column < this.data[row].Count; ++column) {
                element = this.data[row][column];
                accWidth += element.GetSize().x + this.HorizontalSpacing;
                if (pos.x <= accWidth) break;
            }
        }
            
        this.OnClicked(row, column, element, pos);
    }
    
    protected virtual void OnClicked(int row, int column, SEElement element, Vector2 rawPos) {
        
    }
    
}
