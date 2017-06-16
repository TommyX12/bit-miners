using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {
    public static Inventory Current;
    public InventorySlot selected;
    public List<InventorySlot> slots;
    public GameObject UIItemPrefab;
    public GameObject border;
    private void Start()
    {
        Current = this;
        slots = new List<InventorySlot>(GetComponentsInChildren<InventorySlot>());
    }

    public void Drop(UIItem item) {
        item.item.gameObject.SetActive(true);
        item.item.gameObject.transform.position = Util.CameraToWorld(Camera.main, Input.mousePosition, 0);
        Destroy(item.gameObject);
    }

    public void Add(WorldItem item) {
        foreach (InventorySlot Islot in slots) {
            if (Islot.item == null) {
                UIItem uiitem = Instantiate(UIItemPrefab).GetComponent<UIItem>();
                uiitem.transform.SetParent(border.transform);
                uiitem.SetItem(item);
                Islot.setItem(uiitem);
                item.gameObject.SetActive(false);
                return;
            }
        }
    }
}
