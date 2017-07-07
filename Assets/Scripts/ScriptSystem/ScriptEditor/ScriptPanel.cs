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
        
        this.ScrollToStart();
    }
    
    public override void NormalUpdate() {
        base.NormalUpdate();
        
        if (this.gameObject.activeInHierarchy) {
            if (
                Input.GetKeyDown(KeyCode.Return) ||
                Input.GetKeyDown(KeyCode.KeypadEnter)
            ) {
                this.CursorReturn();
            }
            
            if (
                Input.GetKeyDown(KeyCode.Backspace) ||
                Input.GetKeyDown(KeyCode.Delete)
            ) {
                this.CursorDelete();
            }
            
            if (
                Input.GetKeyDown(KeyCode.Space)
            ) {
                this.CursorInsert();
            }
            
            int dRow = 0;
            int dColumn = 0;
            if (Input.GetKeyDown(KeyCode.UpArrow)) dRow--;
            if (Input.GetKeyDown(KeyCode.DownArrow)) dRow++;
            if (Input.GetKeyDown(KeyCode.LeftArrow)) dColumn--;
            if (Input.GetKeyDown(KeyCode.RightArrow)) dColumn++;
            if (dRow != 0 || dColumn != 0) {
                this.CursorMove(dRow, dColumn);
            }
        }
    }
    
    protected override void OnRedraw() {
        this.RedrawCursor();
    }
    
    protected override void OnClicked(int row, int column, SEElement element, Vector2 rawPos) {
        this.SetCursor(row, column);
    }
    
    protected void CursorReturn() {
        if (this.cursorRow >= this.data.Count) {
            this.cursorRow++;
            this.InsertRow(this.cursorRow - 1);
        }
        else if (this.cursorColumn >= this.data[this.cursorRow].Count - 1) {
            this.cursorRow++;
            this.InsertRow(this.cursorRow);
        }
    }
    
    protected void CursorInsert() {
        SEElement element = Util.Make<SEElement>(this.SETextElementPrefab);
        element.SetSize(new Vector2(Util.RandomFloat(10.0f, 100.0f), 100.0f));
        this.cursorColumn++;
        this.InsertElement(this.cursorRow, this.cursorColumn - 1, element);
    }
    
    protected void CursorDelete() {
        if (this.data.Count == 0) return;
        
        if (this.cursorRow >= this.data.Count) {
            this.cursorRow--;
            this.cursorColumn = this.data[this.cursorRow].Count - 1;
            this.CursorDelete();
        }
        else if (this.data[this.cursorRow].Count == 0) {
            this.cursorRow--;
            if (this.cursorRow >= 0) this.cursorColumn = this.data[this.cursorRow].Count - 1;
            this.DeleteRow(this.cursorRow + 1);
        }
        else {
            this.DeleteElement(this.cursorRow, this.cursorColumn);
        }
    }
    
    public void SetCursor(int row, int column) {
        this.cursorRow    = row;
        this.cursorColumn = column;
        this.RedrawCursor();
    }
    
    public void CursorMove(int dRow, int dColumn) {
        this.cursorRow    += dRow;
        this.cursorColumn += dColumn;
        this.RedrawCursor();
    }
    
    private void RedrawCursor() {
        Vector2 pos = new Vector2(0, 0);
        
        this.cursorRow = Util.Clamp(this.cursorRow, 0, this.data.Count);
        
        pos.y = this.cursorRow * (this.ElementHeight + this.VerticalSpacing);
        
        if (this.cursorRow < this.data.Count) {
            if (this.data[this.cursorRow].Count > 0) {
                this.cursorColumn = Util.Clamp(this.cursorColumn, 0, this.data[this.cursorRow].Count - 1);
                for (int i = 0; i <= this.cursorColumn; ++i) {
                    pos.x += this.data[this.cursorRow][i].GetSize().x + this.HorizontalSpacing;
                }
                if (this.data[this.cursorRow].Count > 0) {
                    pos.x -= this.HorizontalSpacing;
                }
            }
        }
        else {
            this.cursorColumn = 0;
        }
        
        this.cursor.SetPosition(pos);
        this.cursor.Reflash();
    }
    
}
