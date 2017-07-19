using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class SEHighlight : SEElement {
    
    private Image Image;
    
    protected float ExtraLength = 25.0f;
    
    public int MinRow {
        get; private set;
    }
    
    public int MaxRow {
        get; private set;
    }
    
    protected override void OnAwake() {
        this.Image = this.GetComponent<Image>();
        Util.TopUIRectTransform(this.transform as RectTransform);
    }
    
    public void SetRows(int row1, int row2) {
        this.MinRow = Math.Min(row1, row2);
        this.MaxRow = Math.Max(row1, row2);
        
        float height = this.Container.GetRowPosition(this.MaxRow) - this.Container.GetRowPosition(this.MinRow) - this.Container.GetVerticalSpacing();
        this.SetPositionAndSize(new Vector2(0.0f, this.Container.GetRowPosition(this.MinRow)), new Vector2(this.ExtraLength, height));
    }
    
    public void SetColor(float[] color) {
        this.SetColor(Util.Float4ToColor(color));
    }
    
    public void SetColor(Color color) {
        this.Image.color = color;
    }
    
    protected override void OnStart() {
        
    }
    
    public override void NormalUpdate() {
        
    }
    
}
