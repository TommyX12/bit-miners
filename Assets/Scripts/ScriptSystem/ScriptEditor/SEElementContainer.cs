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
    [HideInInspector]
    public RectTransform HighlightContainer;
    
    public SEHighlight SEHighlightPrefab;
    
    protected Vector2 size = new Vector2(0.0f, 0.0f);
    protected List<List<SEElement>> data = new List<List<SEElement>>();
    protected List<int> cachedIndents = new List<int>();
    
    protected float ElementHeight     = 22.5f;
    protected float VerticalSpacing   = 3.0f;
    protected float HorizontalSpacing = 3.0f;
    protected Vector2 ExtraEndSpace   = new Vector2(20.0f, 50.0f);
    protected float IndentSize        = 32;
    
    protected IDBasedContainer<SEElementDef> IDContainer = new IDBasedContainer<SEElementDef>();
    
    protected HashSet<SEHighlight> highlights = new HashSet<SEHighlight>();
    
    protected bool Redrawable = true;
    
    private bool cacheDirty = true;
    
    void Awake() {
        Util.SafeGetComponent<Mask>(this.gameObject);
        
        this.Container = Util.MakeEmptyUIContainer((RectTransform)this.transform).GetComponent<RectTransform>();
        this.Container.SetAsFirstSibling();
        Util.TopLeftUIRectTransform(this.Container);
        this.Container.pivot = new Vector2(0.0f, 1.0f);
        
        this.HighlightContainer = Util.MakeEmptyUIContainer(this.Container).GetComponent<RectTransform>();
        this.HighlightContainer.SetAsFirstSibling();
        Util.TopUIRectTransform(this.HighlightContainer);
        this.HighlightContainer.pivot = new Vector2(0.0f, 1.0f);
        
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
    
    public void InsertElement(int row, int column, SEElement element) {
        this.InitElement(element);
        row = Util.Clamp(row, 0, this.data.Count);
        if (row == this.data.Count) {
            this.data.Add(new List<SEElement>());
        }
        if (this.data[row].Count == 0) {
            this.data[row].Add(element);
        }
        else {
            column = Util.Clamp(column, 0, this.data[row].Count - 1);
            Util.SafeInsert(this.data[row], column + 1, element);
        }
        this.Redraw();
    }
    
    public void InitElement(SEElement element) {
        element.Container = this;
        element.transform.SetParent(this.Container.transform, false);
        element.SetHeight(this.ElementHeight);
        element.Init();
        this.IDContainer.AddWithID(element.Definition.ID, element.Definition);
        this.Redraw();
    }
    
    public void InsertElements(int row, int column, SEElement[] elements, int start = 0, int end = -1) {
        this.Redrawable = false;
        if (end == -1) end = elements.Length - 1;
        if (end < start) return;
        for (int i = start; i <= end; ++i) {
            SEElement element = elements[i];
            this.InsertElement(row, column, element);
            column++;
        }
        this.Redrawable = true;
        this.Redraw();
    }
    
    public void InsertRow(int row) {
        Util.SafeInsert(this.data, row, new List<SEElement>());
        this.Redraw();
    }
    
    public void DeleteElement(int row, int column) {
        if (!Util.InRange(row, 0, this.data.Count - 1)) return;
        if (!Util.InRange(column, 0, this.data[row].Count - 1)) return;
        this.data[row][column].Remove();
        this.IDContainer.Remove(this.data[row][column].Definition);
        this.data[row].RemoveAt(column);
        this.Redraw();
    }
    
    public void DeleteElements(int row1, int column1, int row2, int column2) {
        // WARNING: this does not check for some edge cases.
        if (row1 > row2) return;
        
        foreach (SEElement element in this.AllBetween(row1, column1, row2, column2)){
            element.Remove();
            this.IDContainer.Remove(element.Definition);
        }
        if (row1 == row2) {
            this.data[row1].RemoveRange(column1, column2 - column1 + 1);
        }
        else {
            
            this.data[row1].RemoveRange(column1, this.data[row1].Count - column1);
            this.data[row2].RemoveRange(0, column2 + 1);
            this.data[row1].AddRange(this.data[row2]);
            this.data.RemoveAt(row2);
            this.data.RemoveRange(row1 + 1, row2 - row1 - 1);
        }
        this.Redraw();
    }
    
    public IEnumerable<SEElement> AllBetween(int row1, int column1, int row2, int column2) {
        if (row1 > row2) yield break;
        if (row1 == row2) {
            for (int i = column1; i <= column2; ++i) {
                yield return this.data[row1][i];
            }
        }
        else {
            for (int i = column1; i < this.data[row1].Count; ++i) {
                yield return this.data[row1][i];
            }
            for (int i = row1 + 1; i < row2; ++i) {
                for (int j = 0; j < this.data[i].Count; ++j) {
                    yield return this.data[i][j];
                }
            }
            for (int i = 0; i <= column2; ++i) {
                yield return this.data[row2][i];
            }
        }
    }
    
    public IEnumerable<SEElement> Elements() {
        foreach (var row in this.data) {
            foreach (var element in row) {
                yield return element;
            }
        }
    }
    
    public void ClearElements() {
        for (int i = 0; i < this.data.Count; ++i) {
            for (int j = 0; j < this.data[i].Count; ++j) {
                this.data[i][j].Remove();
            }
        }
        this.IDContainer.Clear();
        this.data.Clear();
        this.Redraw();
    }
    
    public void DeleteRow(int row) {
        if (!Util.InRange(row, 0, this.data.Count - 1)) return;
        foreach (SEElement element in this.data[row]) {
            element.Remove();
            this.IDContainer.Remove(element.Definition);
        }
        this.data.RemoveAt(row);
        this.Redraw();
    }
    
    public SEElement GetElementByID(int id) {
        return this.IDContainer.Get(id).Element;
    }
    
    public SEElement GetElement(int row, int column) {
        return this.data[row][column];
    }
    
    public int GetRowOf(SEElement element) {
        this.RefreshCacheIfDirty();
        return element.cachedRow;
    }
    
    public int GetColumnOf(SEElement element) {
        this.RefreshCacheIfDirty();
        return element.cachedColumn;
    }
    
    public float GetRowPosition(int row) {
        return row * (this.ElementHeight + this.VerticalSpacing);
    }
    
    public float GetVerticalSpacing() {
        return this.VerticalSpacing;
    }
    
    public void RefreshCacheIfDirty() {
        if (!this.cacheDirty) return;
        for (int i = 0; i < this.data.Count; ++i) {
            for (int j = 0; j < this.data[i].Count; ++j) {
                this.data[i][j].SetCache(i, j);
            }
        }
        this.cacheDirty = false;
    }
    
    public Vector2 GetSize() {
        return this.size;
    }
    
    public void SetSize(Vector2 size) {
        this.size = size;
        this.UpdatePosition();
    }
    
    public void Redraw() {
        if (!this.Redrawable) return;
        this.cacheDirty = true;
        
        Vector2 pos = new Vector2(0.0f, 0.0f);
        float rowHeight = this.ElementHeight + this.VerticalSpacing;
        
        Util.ResizeList(this.cachedIndents, this.data.Count, 0);
        
        float maxWidth = 0.0f;
        int indent = 0;
        int indentMod = 0;
        int i = 0;
        foreach (var row in this.data) {
            indent += indentMod;
            indentMod = 0;
            
            foreach (var element in row) {
                if (element.Definition.IndentMod > 0) {
                    indentMod += element.Definition.IndentMod;
                }
                else {
                    indent += element.Definition.IndentMod;
                }
            }
            this.cachedIndents[i] = indent;
            
            pos.x = this.IndentSize * indent;
            foreach (var element in row) {
                element.SetPosition(pos);
                pos.x += element.GetSize().x + this.HorizontalSpacing;
            }
            maxWidth = Mathf.Max(maxWidth, pos.x);
            pos.y += rowHeight;
            
            i++;
        }
        
        this.SetSize(new Vector2(maxWidth, pos.y) + this.ExtraEndSpace);
        
        this.OnRedraw();
    }
    
    public float GetIndentOf(int row) {
        if (!Util.InRange(row, 0, this.cachedIndents.Count - 1)) {
            return 0;
        }
        return this.cachedIndents[row] * this.IndentSize;
    }
    
    protected virtual void OnRedraw() {
        
    }
    
    private void UpdatePosition() {
        Vector2 scrollPosition = this.ScrollRect.normalizedPosition;
        this.Container.offsetMin = new Vector2(
            0.0f, -this.size.y
        );
        this.Container.offsetMax = new Vector2(
            this.size.x, 0.0f
        );
        this.ScrollRect.normalizedPosition = scrollPosition;
    }
    
    public void ScrollToStart() {
        this.ScrollRect.verticalNormalizedPosition = 1.0f;
    }
    
    public void ScrollToEnd() {
        this.ScrollRect.verticalNormalizedPosition = 0.0f;
    }
    
    public override void NormalUpdate() {
        
    }
    
    public SEHighlight AddHighlight(int row1, int row2, float[] color) {
        SEHighlight highlight = (SEHighlight)Util.Make(this.SEHighlightPrefab);
        highlight.Container = this;
        highlight.transform.SetParent(this.HighlightContainer, false);
        highlight.SetRows(row1, row2);
        highlight.SetColor(color);
        this.highlights.Add(highlight);
        
        return highlight;
    }
    
    public void RemoveHighlight(SEHighlight highlight) {
        this.highlights.Remove(highlight);
        highlight.Remove();
    }
    
    public void ClearHighlight() {
        foreach (var highlight in this.highlights) {
            highlight.Remove();
        }
        this.highlights.Clear();
    }
    
    private void OnPointerDown(PointerEventData data) {
        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(this.Container.transform as RectTransform, data.position, data.pressEventCamera, out pos);
        
        int row = (int)Math.Floor(-pos.y / (this.ElementHeight + this.VerticalSpacing));
        row = Util.Clamp(row, 0, this.data.Count);
        int column = 0;
        SEElement element = null;
        if (row < this.data.Count) {
            float accWidth = this.GetIndentOf(row);
            for (; column < this.data[row].Count; ++column) {
                element = this.data[row][column];
                accWidth += element.GetSize().x + this.HorizontalSpacing;
                if (SEBlockDef.GetRegionFlag(element.Definition.RegionType) == 0) continue;
                if (pos.x <= accWidth) break;
            }
        }
            
        this.OnClicked(row, column, element, pos);
    }
    
    protected virtual void OnClicked(int row, int column, SEElement element, Vector2 rawPos) {
        
    }
    
}
