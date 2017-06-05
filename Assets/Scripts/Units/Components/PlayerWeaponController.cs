using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerWeaponController : MyMono {

    public WeaponComponent weapon;

    public override void PausingUpdate()
    {
        base.PausingUpdate();

        if (weapon == null) {
            return;
        }

        weapon.AimAtPosition(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        if (Input.GetMouseButton(0) && !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject()) {
            weapon.fire();
        }
    }
}
