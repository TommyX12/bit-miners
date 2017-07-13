using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class SETextElement: SEElement {
    
    public Text TextObject;
    public Image ImageObject;
    
    protected static float FlashingSpeed = 10.0f;
    
    protected override void OnAwake() {
        this.ImageObject = Util.SafeGetComponent<Image>(this.gameObject);
    }
    
    protected override void OnStart() {
        
    }
    
    public override void NormalUpdate() {
        if (this.active) {
            this.Definition.Color[3] = Util.Flashing(Time.time, 0.85f, 0.95f, FlashingSpeed);
            this.ImageObject.color = Util.Float4ToColor(this.Definition.Color);
        }
    }
    
    public override void SetActive(bool active) {
        base.SetActive(active);
        if (active) {
            
        }
        else {
            this.Definition.Color[3] = 0.5f;
        }
        this.ImageObject.color = Util.Float4ToColor(this.Definition.Color);
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
        this.TextObject.color = Util.Float4ToColor(Util.GetTextColorBW(this.Definition.Color, 0.75f));
        this.ImageObject.color = Util.Float4ToColor(this.Definition.Color);
        this.SetWidth(this.GetTextWidth(this.Definition.Text) + 10);
    }
    
}
