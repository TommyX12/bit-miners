using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class ScriptPanel: SEElementContainer {
    
    public SETextElement SETextElementPrefab;
    public SECursor SECursorPrefab;
    
    private SECursor cursor;
    private int cursorRow = 0;
    private int cursorColumn = 0;
    
    [HideInInspector]
    protected float CursorWidth = 4.0f;
    
    protected override void OnAwake() {
        
        this.cursor = Util.MakeChild(this.transform, SECursorPrefab);
        this.cursor.transform.SetParent(this.Container.transform, false);
        Util.InitUIRectTransform(this.cursor.transform as RectTransform);
        Util.TopLeftUIRectTransform(this.cursor.transform as RectTransform);
        
    }
    
    protected override void OnStart() {
        this.cursor.SetSize(new Vector2(this.CursorWidth, this.ElementHeight));
        
        SEElement element;
        element = Util.Make<SEElement>(this.SETextElementPrefab);
        this.AddElement(0, 0, element);
        element = Util.Make<SEElement>(this.SETextElementPrefab);
        element.SetSize(new Vector2(100.0f, 100.0f));
        this.AddElement(0, 0, element);
        element = Util.Make<SEElement>(this.SETextElementPrefab);
        element.SetSize(new Vector2(150.0f, 100.0f));
        for (int i = 1; i < 50; ++i){
            element = Util.Make<SEElement>(this.SETextElementPrefab);
            element.SetSize(new Vector2(Util.RandomFloat(100.0f, 600.0f), 100.0f));
            this.AddElement(i, 0, element);
        }
    }
    
    public override void NormalUpdate() {
        base.NormalUpdate();
    }
    
    protected override void OnRedraw() {
        this.RedrawCursor();
    }
    
    protected override void OnClicked(int row, int column, SEElement element, Vector2 rawPos) {
        this.SetCursor(row, column);
    }
    
    public void SetCursor(int row, int column) {
        this.cursorRow    = row;
        this.cursorColumn = column;
        this.RedrawCursor();
    }
    
    private void RedrawCursor() {
        Vector2 pos = new Vector2(0, 0);
        
        this.cursorRow = Util.Clamp(this.cursorRow, 0, this.data.Count);
        
        pos.y = this.cursorRow * (this.ElementHeight + this.VerticalSpacing);
        
        if (this.cursorRow < this.data.Count) {
            this.cursorColumn = Util.Clamp(this.cursorColumn, 0, this.data[this.cursorRow].Count - 1);
            for (int i = 0; i <= this.cursorColumn; ++i) {
                pos.x += this.data[this.cursorRow][i].GetSize().x + this.HorizontalSpacing;
            }
            if (this.data[this.cursorRow].Count > 0) {
                pos.x -= this.HorizontalSpacing;
            }
        }
        else {
            this.cursorColumn = 0;
        }
        
        this.cursor.SetPosition(pos);
        this.cursor.Reflash();
    }
    
}
