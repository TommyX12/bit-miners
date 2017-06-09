using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : UnitComponent {
    public RectTransform Health;
    public float test;

    public void Refresh(float percent)
    {
        Health.anchorMax = new Vector2(percent, 1);
        Health.anchorMin = new Vector2(0, 0);
        Health.sizeDelta = new Vector2(0, 0);
        Health.offsetMax = new Vector2(0, 0);
        Health.offsetMin = new Vector2(0, 0);
    }

}
