using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingSelector : MyMono {

    public ProductionUI UI;

    public override void PausingUpdate()
    {
        base.PausingUpdate();

        if (!UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject()) {

            if (Input.GetMouseButtonDown(0)) {
                RaycastHit2D[] hits = Physics2D.RaycastAll(Util.CameraToWorld(Camera.main, Input.mousePosition, 0), Vector2.zero, Mathf.Infinity);
                foreach (RaycastHit2D hit in hits)
                {
                    if (hit.collider != null)
                    {
                        ProductionComponent prod;
                        if ((prod = hit.collider.gameObject.GetComponentInChildren<ProductionComponent>()) != null)
                        {
                            UI.gameObject.SetActive(true);
                            UI.SetProductionComponent(prod);
                            return;
                        }
                        else
                        {
                            UI.gameObject.SetActive(false);
                        }
                    }
                    else
                    {
                        UI.gameObject.SetActive(false);
                    }
                }
            }
        }
    }
}
