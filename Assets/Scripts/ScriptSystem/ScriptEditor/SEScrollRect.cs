using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SEScrollRect: ScrollRect {
    protected bool AllowDrag = false;
    
    public override void OnBeginDrag(PointerEventData eventData) {
        if (this.AllowDrag) {
            base.OnBeginDrag(eventData);
        }
    }
    
    public override void OnDrag(PointerEventData eventData) {
        if (this.AllowDrag) {
            base.OnDrag(eventData);
        }
    }
    
    public override void OnEndDrag(PointerEventData eventData) {
        if (this.AllowDrag) {
            base.OnEndDrag(eventData);
        }
    }
    
}
