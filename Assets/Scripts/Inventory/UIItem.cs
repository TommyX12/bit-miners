using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
[RequireComponent(typeof(CanvasGroup))]
[RequireComponent(typeof(SpriteRenderer))]
public class UIItem : MonoBehaviour {
    bool dragging = false;
    RectTransform rectTransform;
    CanvasGroup canvasGroup;
    public InventorySlot slot;
    public WorldItem item;
    public Image rendy;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        rendy = GetComponent<Image>();
    }

    private void Update()
    {
        if (dragging) {
            rectTransform.position = Input.mousePosition;
        }
    }

    public void BeginDrag() {
        dragging = true;
        canvasGroup.blocksRaycasts = false;
        canvasGroup.interactable = false;
        slot.removeItem();
    }

    public void EndDrag() {
        if (Inventory.Current.selected == null)
        {
            Inventory.Current.Drop(this);
        }
        else {
            Inventory.Current.selected.setItem(this);
        }
        dragging = false;
        canvasGroup.blocksRaycasts = true;
        canvasGroup.interactable = true;
    }

    public void SetItem(WorldItem item) {
        Start();
        this.item = item;
        rendy.sprite = item.gameObject.GetComponent<SpriteRenderer>().sprite;
    }
}
