using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class SEAPIElement: SEElement {
    
    public Text TextObject;
    public Image ImageObject;
    
    protected static float FlashingSpeed = 10.0f;
    
    protected override void OnAwake() {
        this.ImageObject = Util.SafeGetComponent<Image>(this.gameObject);
    }
    
    protected override void OnStart() {
        
    }
    
    public override void NormalUpdate() {
        if (this.Definition.BlockDefName == null) return;
        if (this.active) {
            this.Definition.Color[3] = Util.Flashing(Time.time, 0.7f, 1.0f, FlashingSpeed);
            this.ImageObject.color = Util.Float4ToColor(this.Definition.Color);
        }
    }
    
    public override void SetActive(bool active) {
        base.SetActive(active);
        
        if (active) {
            this.TextObject.color = Color.white;
        }
        else {
            if (this.Definition.BlockDefName == null) {
                this.TextObject.color = Color.gray;
            }
            else {
                this.Definition.Color[3] = 0.3f;
                this.TextObject.color = Color.black;
                this.ImageObject.color = Util.Float4ToColor(this.Definition.Color);
            }
        }
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
        this.TextObject.color = Color.white;
        this.ImageObject.color = Util.Float4ToColor(this.Definition.Color);
        this.SetWidth(this.GetTextWidth(this.Definition.Text) + 10);
        if (this.Definition.BlockDefName == null) {
            this.TextObject.fontStyle = FontStyle.Bold;
        }
    }
    
}
