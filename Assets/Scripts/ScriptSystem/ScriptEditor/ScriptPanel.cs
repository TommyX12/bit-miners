using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;

public class ScriptPanel: SEElementContainer {
    
    public SETextElement SETextElementPrefab;
    public SEInputElement SEInputElementPrefab;
    public SECursor SECursorPrefab;
    
    private SECursor cursor;
    private int cursorRow = 0;
    private int cursorColumn = 0;
    
    [HideInInspector]
    protected float CursorWidth = 4.0f;
    
    private bool flagsDirty = true;
    private int cachedFlags = 0;
    
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
    
    public bool HasFocusedInput() {
        foreach (var element in this.Elements()) {
            if (element is SEInputElement) {
                if (((SEInputElement)element).IsFocused()) {
                    return true;
                }
            }
        }
        
        return false;
    }
    
    public void CursorReturn() {
        if (this.HasFocusedInput()) return;
        
        if (this.cursorRow >= this.data.Count) {
            this.cursorRow++;
            this.InsertRow(this.cursorRow - 1);
        }
        else if (this.cursorColumn >= this.data[this.cursorRow].Count - 1) {
            this.cursorRow++;
            this.InsertRow(this.cursorRow);
        }
    }
    
    public void CursorInsert() {
        if (this.HasFocusedInput()) return;
        
        if (this.cursorRow < this.data.Count && this.data[this.cursorRow].Count > 0 && this.cursorColumn >= this.data[this.cursorRow].Count - 1) {
            this.CursorReturn();
        }
            
        SEElement element = Util.Make<SEElement>(this.SETextElementPrefab);
        element.SetSize(new Vector2(Util.RandomFloat(10.0f, 100.0f), 100.0f));
        this.cursorColumn++;
        this.InsertElement(this.cursorRow, this.cursorColumn - 1, element);
    }
    
    public void CursorInsertBlock(SEBlockDef blockDef) {
        if (this.HasFocusedInput()) return;
        
        if (this.cursorRow < this.data.Count && this.data[this.cursorRow].Count > 0 && this.cursorColumn >= this.data[this.cursorRow].Count - 1) {
            this.CursorReturn();
        }
            
        SEElement[] elements = new SEElement[blockDef.Elements.Length];
        
        float[] color = SEBlockDef.GetTypeColor(blockDef.Type);
            
        for (int i = 0; i < elements.Length; ++i) {
            blockDef.Elements[i].Color = color;
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
            elements[i].Definition.Color = color;
            children[i] = elements[i].Definition.ID;
        }
        elements[0].Definition.Children = children;
        elements[0].Definition.BlockDefName = blockDef.Name;
        
        // reset cursor to desired position
        this.SetCursor(
            this.GetRowOf(elements[blockDef.CursorIndex]),
            this.GetColumnOf(elements[blockDef.CursorIndex])
        );
    }
    
    public void CursorDelete() {
        if (this.HasFocusedInput()) return;
        
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
        this.flagsDirty = true;
        
        Vector2 pos = new Vector2(this.GetIndentOf(this.cursorRow), 0);
        
        this.cursorRow = Util.Clamp(this.cursorRow, 0, this.data.Count);
        
        pos.y = this.cursorRow * (this.ElementHeight + this.VerticalSpacing);
        
        foreach (var row in this.data) {
            foreach (var element in row) {
                element.SetActive(false);
            }
        }
        
        if (this.cursorRow < this.data.Count) {
            if (this.data[this.cursorRow].Count > 0) {
                this.cursorColumn = Util.Clamp(this.cursorColumn, 0, this.data[this.cursorRow].Count - 1);
                for (int i = 0; i <= this.cursorColumn; ++i) {
                    pos.x += this.data[this.cursorRow][i].GetSize().x + this.HorizontalSpacing;
                }
                if (this.data[this.cursorRow].Count > 0) {
                    pos.x -= this.HorizontalSpacing;
                    
                    int parentID = this.data[this.cursorRow][this.cursorColumn].Definition.ParentID;
                    if (parentID >= 0) {
                        SEElement startElement = this.GetElementByID(this.GetElement(this.cursorRow, this.cursorColumn).Definition.ParentID);
                        SEElement endElement = this.GetElementByID(startElement.Definition.Children[startElement.Definition.Children.Length - 1]);
                        IEnumerable<SEElement> elements = this.AllBetween(
                            this.GetRowOf(startElement),
                            this.GetColumnOf(startElement),
                            this.GetRowOf(endElement),
                            this.GetColumnOf(endElement)
                        );
                        foreach (SEElement element in elements) {
                            element.SetActive(true);
                        }
                    }
                }
            }
        }
        else {
            this.cursorColumn = 0;
        }
        
        this.cursor.SetPosition(pos);
        this.cursor.Reflash();
    }
    
    public int GetCursorFlags() {
        if (!this.flagsDirty) return this.cachedFlags;
        
        this.flagsDirty = false;
        
        int row = this.cursorRow;
        int column = this.cursorColumn;
        this.ElementClosestTo(ref row, ref column);
        
        int result = SEBlockDef.GetRegionFlag("top");
        bool jumped = false;
        
        while (true) {
            if (!Util.InRange(row, 0, this.data.Count - 1)) {
                break;
            }
            SEElementDef elementDef = this.GetElement(row, column).Definition;
            int flags = SEBlockDef.GetRegionFlag(elementDef.RegionType);
            if (flags == -1) {
                this.ParentOf(ref row, ref column);
                this.OneElementBackward(ref row, ref column);
                this.ElementClosestTo(ref row, ref column);
                jumped = true;
            }
            else {
                if (jumped && !elementDef.MultiRegion) result = SEBlockDef.GetRegionFlag("none");
                else result = flags;
                break;
            }
        }
        
        this.cachedFlags = result;
        
        return this.cachedFlags;
    }
    
    public void OneElementBackward(ref int row, ref int column) {
        column--;
        while (column < 0) {
            row--;
            if (Util.InRange(row, 0, this.data.Count - 1)) {
                column = this.data[row].Count - 1;
            }
            else {
                column = 0;
                break;
            }
        }
    }
    
    public void ElementClosestTo(ref int row, ref int column) {
        if (row >= this.data.Count) {
            OneElementBackward(ref row, ref column);
        }
        while (Util.InRange(row, 0, this.data.Count - 1)) {
            if (this.data[row].Count > 0) {
                if (column < 0) {
                    column = this.data[row].Count - 1;
                }
                return;
            }
            else {
                row--;
                column = -1;
            }
        }
        column = 0;
    }
    
    public void ParentOf(ref int row, ref int column) {
        // WARNING: this does not check for some edge cases.
        SEElement parent = this.GetElementByID(this.GetElement(row, column).Definition.ParentID);
        row = this.GetRowOf(parent);
        column = this.GetColumnOf(parent);
    }
    
    public string SaveString() {
        List<List<SEElementDef>> defList = new List<List<SEElementDef>>();
        foreach (var row in this.data) {
            List<SEElementDef> defListRow = new List<SEElementDef>();
            foreach (var element in row) {
                element.BeforeSave();
                defListRow.Add(element.Definition);
            }
            defList.Add(defListRow);
        }
        return JsonConvert.SerializeObject(defList);
    }
    
    public void LoadString(string json) {
        this.ClearElements();
        
        List<List<SEElementDef>> defList = JsonConvert.DeserializeObject<List<List<SEElementDef>>>(json);
        
        this.Redrawable = false;
        foreach (var row in defList) {
            List<SEElement> dataRow = new List<SEElement>();
            foreach (var elementDef in row) {
                SEElement element = elementDef.SpawnElement(this.GetPrefab);
                dataRow.Add(element);
                this.InitElement(element);
            }
            this.data.Add(dataRow);
        }
        this.Redrawable = true;
        
        this.Redraw();
    }
    
    private SEElement GetPrefab(string elementType) {
        switch (elementType) {
            case "text":
                return this.SETextElementPrefab;
                break;
        
            case "input":
                return this.SEInputElementPrefab;
                break;
        
            default:
                return null;
                break;
        }
    }
    
}
