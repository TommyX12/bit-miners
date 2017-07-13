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
        Text placeholder = this.InputFieldObject.placeholder.GetComponent<Text>();
        switch (this.Definition.InputType) {
            case "num":
                this.InputFieldObject.contentType = InputField.ContentType.DecimalNumber;
                this.SetWidth(60);
                placeholder.text = "(num)";
                break;
        
            case "id":
                this.InputFieldObject.contentType = InputField.ContentType.Alphanumeric;
                this.SetWidth(75);
                placeholder.text = "(name)";
                break;
        
            case "str":
                this.InputFieldObject.contentType = InputField.ContentType.Autocorrected;
                this.SetWidth(100);
                placeholder.text = "(text)";
                break;
                
            case "comment":
                this.InputFieldObject.contentType = InputField.ContentType.Autocorrected;
                this.SetWidth(250);
                placeholder.text = "(text)";
                break;
                
            case "js":
                this.InputFieldObject.contentType = InputField.ContentType.Standard;
                this.SetWidth(250);
                placeholder.text = "(script)";
                break;
        }
    }
    
    public override void BeforeSave() {
        this.Definition.Text = this.InputFieldObject.text;
    }
    
}
