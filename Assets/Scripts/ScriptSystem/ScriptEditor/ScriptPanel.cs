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
                Input.GetKeyDown(KeyCode.I)
            ) {
                this.CursorInsertBlock("if");
            }
            
            if (
                Input.GetKeyDown(KeyCode.M)
            ) {
                this.CursorInsertBlock("api_move_to");
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
    
    protected void CursorInsertBlock(string blockName) {
        SEBlockDef blockDef = SEBlockDef.GetPreset(blockName);
        SEElement[] elements = new SEElement[blockDef.Elements.Length];
        for (int i = 0; i < elements.Length; ++i) {
            elements[i] = blockDef.Elements[i].SpawnElement(this.GetPrefab);
        }
        int rangeStart = 0;
        int rangeEnd;
        for (rangeEnd = 0; rangeEnd < elements.Length; ++rangeEnd) {
            if (elements[rangeEnd].Definition.RegionType == "block") {
                this.InsertElements(this.cursorRow, this.cursorColumn, elements, rangeStart, rangeEnd);
                this.cursorColumn += rangeEnd - rangeStart + 1;
                this.CursorReturn();
                this.CursorReturn();
                rangeStart = rangeEnd + 1;
            }
        }
        this.InsertElements(this.cursorRow, this.cursorColumn, elements, rangeStart, rangeEnd - 1);
        
        int[] children = new int[blockDef.Elements.Length];
        for (int i = 0; i < elements.Length; ++i) {
            elements[i].Definition.ParentID = elements[0].Definition.ID;
            children[i] = elements[i].Definition.ID;
        }
        elements[0].Definition.Children = children;
        
        // reset cursor to desired position
        this.SetCursor(
            this.GetRowOf(elements[blockDef.CursorIndex]),
            this.GetColumnOf(elements[blockDef.CursorIndex])
        );
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
            SEElement startElement = this.GetElementByID(this.GetElement(this.cursorRow, this.cursorColumn).Definition.ParentID);
            SEElement endElement = this.GetElementByID(startElement.Definition.Children[startElement.Definition.Children.Length - 1]);
            this.cursorRow = this.GetRowOf(startElement);
            this.cursorColumn = this.GetColumnOf(startElement) - 1;
            this.DeleteElements(
                this.GetRowOf(startElement),
                this.GetColumnOf(startElement),
                this.GetRowOf(endElement),
                this.GetColumnOf(endElement)
            );
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
    
    private Component GetPrefab(string elementType) {
        switch (elementType) {
            case "text":
                return this.SETextElementPrefab;
                break;
        
            default:
                return null;
                break;
        }
    }
    
}
