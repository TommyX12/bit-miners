using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class SEAPIElement: SEElement {
    
    public Text TextObject;
    public Image ImageObject;
    
    protected override void OnAwake() {
        this.ImageObject = Util.SafeGetComponent<Image>(this.gameObject);
    }
    
    protected override void OnStart() {
        
    }
    
    public override void PausingUpdate() {
        
    }
    
    public static Font cachedFont = null;
    
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
        
        return text.Length * 7.5f;
    }
    
    public override void SetupDefinition() {
        this.TextObject.text = this.Definition.Text;
        this.TextObject.color = Util.Float4ToColor(this.Definition.TextColor);
        this.ImageObject.color = Util.Float4ToColor(this.Definition.Color);
        this.SetWidth(this.GetTextWidth(this.Definition.Text) + 10);
    }
    
}
