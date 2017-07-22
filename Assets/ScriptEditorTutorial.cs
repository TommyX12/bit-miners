using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptEditorTutorial : MonoBehaviour {
    public Color PanelColor;

    public static ScriptEditorTutorial Current;

    public Dictionary<string, bool> conditions;
    public DialogueDisplay display;

    public ScriptTutorialObject active;
    public ScriptTutorialObject start;

    public ColorBlink CanProceedIndicator;

    public void Start()
    {
        conditions = new Dictionary<string, bool>();
        Current = this;
        display.Show(start.text);
    }

    public void next()
    {
        if (check(active.conditions))
        {
            active.next.last = active;
            active.gameObject.SetActive(false);
            active = active.next;
            active.gameObject.SetActive(true);
        }
        display.Show(active.text);
    }

    public void last()
    {
        if (active.last != null)
        {
            active.gameObject.SetActive(false);
            active = active.last;
            active.gameObject.SetActive(true);
        }
        display.Show(active.text);
    }

    public bool check(List<string> bools)
    {
        foreach (string s in bools)
        {
            if (conditions.ContainsKey(s))
            {
                if (conditions[s])
                {
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        return true;
    }

    public void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            next();
        }

        if (Input.GetKey(KeyCode.LeftControl))
        {
            next();
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            last();
        }

        if (check(active.conditions))
        {
            CanProceedIndicator.On = true;
        }
        else
        {
            CanProceedIndicator.On = false;
        }
    }

    public void Restart() {
        GameManager.Current.ScriptEditorV2Object.ScriptPanelObject.ClearElements();
        active.gameObject.SetActive(false);
        active = start;
        display.Show(active.text);
        active.gameObject.SetActive(true);
        gameObject.SetActive(true);
        foreach (string key in new List<string>(conditions.Keys)) {
            conditions[key] = false;
        }
    }

    public void SetOrAdd(string key, bool value)
    {
        if (conditions.ContainsKey(key))
        {
            conditions[key] = value;
        }
        else
        {
            conditions.Add(key, value);
        }
    }
}
