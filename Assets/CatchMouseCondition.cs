using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(RectTransform))]
public class CatchMouseCondition : MonoBehaviour {

    public RectTransform tranny;
    public string conditionToActivate;
    public bool value;

    private void Start()
    {
    }

    private void Update()
    {
        if (Input.GetMouseButtonUp(0)) {
            if (RectTransformUtility.RectangleContainsScreenPoint(tranny, Input.mousePosition)) {
                ScriptEditorTutorial.Current.SetOrAdd(conditionToActivate, value);
            }
        }
    }
}
