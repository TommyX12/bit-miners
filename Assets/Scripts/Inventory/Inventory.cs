﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {
    public static Inventory Current;
    public InventorySlot selected;
    public List<InventorySlot> slots;
    public GameObject UIItemPrefab;
    public GameObject border;

    private void Awake()
    {
        Current = this;
    }

    private void Start()
    {
        slots = new List<InventorySlot>(GetComponentsInChildren<InventorySlot>());
    }

    public void Drop(UIItem item) {

        Vector3 worldPosition = Util.CameraToWorld(Camera.main, Input.mousePosition, 0);
        Vector3 dropPosition = Camera.main.transform.position;
        dropPosition.z = 0;
        Unit unit;
        if ((unit = Util.RaycastAndFilter<Unit>(worldPosition)) != null) {
            WorldItem world = item.item;
            item.item.gameObject.SetActive(true);
            item.item.gameObject.transform.position = dropPosition;
            item.slot.removeItem();
            Destroy(item.gameObject);
            world.DroppedOnUnit(unit);
            return;
        }
        item.slot.removeItem();
        item.item.gameObject.SetActive(true);
        item.item.gameObject.transform.position = dropPosition;
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
