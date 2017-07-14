using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class SEInputElement: SEElement {
    
    public InputField InputFieldObject;
    
    protected float minWidth = 50;
    protected float widthShrinkBuffer = 50;
    
    protected override void OnAwake() {
        this.InputFieldObject = this.GetComponent<InputField>();
        this.InputFieldObject.onValueChanged.AddListener(this.OnTextChanged);
    }
    
    public bool IsFocused() {
        return this.InputFieldObject.isFocused;
    }
    
    public override void SetupDefinition() {
        this.InputFieldObject.text = this.Definition.Text;
        Text placeholder = this.InputFieldObject.placeholder.GetComponent<Text>();
        switch (this.Definition.InputType) {
            case "num":
                this.InputFieldObject.contentType = InputField.ContentType.DecimalNumber;
                this.minWidth = 50;
                placeholder.text = "(num)";
                break;
        
            case "id":
                this.InputFieldObject.contentType = InputField.ContentType.Alphanumeric;
                this.minWidth = 50;
                placeholder.text = "(name)";
                break;
        
            case "str":
                this.InputFieldObject.contentType = InputField.ContentType.Autocorrected;
                this.minWidth = 50;
                placeholder.text = "(text)";
                break;
                
            case "comment":
                this.InputFieldObject.contentType = InputField.ContentType.Autocorrected;
                this.minWidth = 50;
                placeholder.text = "(text)";
                break;
                
            case "js":
                this.InputFieldObject.contentType = InputField.ContentType.Standard;
                this.minWidth = 60;
                placeholder.text = "(script)";
                break;
        }
        this.SetWidth(this.minWidth);
    }
    
    public override void BeforeSave() {
        base.BeforeSave();
        this.Definition.Text = this.InputFieldObject.text;
    }
    
    public float GetTextWidth(string text) {
        /* int totalLength = 0;

        if (cachedFont == null) cachedFont = this.TextObject.font;
        CharacterInfo characterInfo = new CharacterInfo();

        char[] arr = text.ToCharArray();

        foreach (char c in arr) {
            cachedFont.GetCharacterInfo(c, out characterInfo, this.TextObject.fontSize);  
            totalLength += characterInfo.advance;
        }

        return totalLength; */
        
        return text.Length * 7.8f;
    }
    
    public void OnTextChanged(string info) {
        if (this.Container == null) return;
        
        float newWidth = Mathf.Max(this.GetTextWidth(info) + 12, this.minWidth);
        float oldWidth = this.GetSize().x;
        int newWidthSteps = (int)Math.Max(0, Math.Ceiling((newWidth - minWidth) / this.widthShrinkBuffer));
        int oldWidthSteps = (int)Math.Max(0, Math.Ceiling((oldWidth - minWidth) / this.widthShrinkBuffer));
        if (newWidthSteps != oldWidthSteps) {
            this.SetWidth(this.minWidth + newWidthSteps * this.widthShrinkBuffer);
            this.Container.Redraw();
        }
    }
    
    public override void Remove() {
        base.Remove();
        this.InputFieldObject.onValueChanged.RemoveListener(this.OnTextChanged);
    }
    
    public override void Init() {
        base.Init();
        this.OnTextChanged(this.InputFieldObject.text);
    }
    
}
