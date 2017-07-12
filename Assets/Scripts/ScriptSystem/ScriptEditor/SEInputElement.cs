using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class SEInputElement: SEElement {
    
    public InputField InputFieldObject;
    
    protected override void OnAwake() {
        this.InputFieldObject = this.GetComponent<InputField>();
    }
    
    public bool IsFocused() {
        return this.InputFieldObject.isFocused;
    }
    
    public override void SetupDefinition() {
        this.InputFieldObject.text = this.Definition.Text;
        switch (this.Definition.InputType) {
            case "num":
                this.InputFieldObject.contentType = InputField.ContentType.DecimalNumber;
                this.SetWidth(60);
                break;
        
            case "id":
                this.InputFieldObject.contentType = InputField.ContentType.Alphanumeric;
                this.SetWidth(75);
                break;
        
            case "str":
            default:
                this.InputFieldObject.contentType = InputField.ContentType.Autocorrected;
                this.SetWidth(100);
                break;
        }
    }
    
    public override void BeforeSave() {
        this.Definition.Text = this.InputFieldObject.text;
    }
    
}
