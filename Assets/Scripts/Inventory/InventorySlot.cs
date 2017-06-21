using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySlot : MonoBehaviour {
    public UIItem item;

    public void setItem(UIItem item) {
        item.transform.position = transform.position;
        item.slot = this;
        this.item = item;
    }

    public void removeItem() {
        item = null;
    }

    public void Select() {
        Inventory.Current.selected = this;
    }

    public void Deselect() {
        if (Inventory.Current.selected = this)
        {
            Inventory.Current.selected = null;
        }
    }
}
