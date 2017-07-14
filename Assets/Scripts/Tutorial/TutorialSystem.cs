using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialSystem : MyMono {

    public static TutorialSystem Current;

    public Dictionary<string, bool> conditions;
    public DialogueDisplay display;

    public Dialogue active;
    public Dialogue start;

    public ColorBlink CanProceedIndicator;

    public void Start()
    {
        conditions = new Dictionary<string, bool>();
        Current = this;
        start.Run();
        display.Show(start.text);
        display.portrait.sprite = active.portrait;
        if (active.sound != null)
        {
            AudioSource.PlayClipAtPoint(active.sound, Camera.main.transform.position);
        }
    }

    public void next() {
        if (check(active.conditions))
        {
            active.next.last = active;
            active = active.next;
            active.Run();

            display.portrait.sprite = active.portrait;
            if (active.sound != null) {
                AudioSource.PlayClipAtPoint(active.sound, Camera.main.transform.position);
            }
        }
        display.Show(active.text);
    }

    public void last() {
        if (active.last != null) {
            active = active.last;
        }
        display.Show(active.text);
        display.portrait.sprite = active.portrait;
    }

    public bool check(List<string> bools){
        foreach (string s in bools) {
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
            else {
                return false;
            }
        }
        return true;
    }

    public override void PausingFixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Space)) {
            next();
        }

        if (Input.GetKey(KeyCode.LeftControl)) {
            next();
        }

        if (Input.GetKeyDown(KeyCode.Backspace)) {
            last();
        }

        if (check(active.conditions))
        {
            CanProceedIndicator.On = true;
        }
        else {
            CanProceedIndicator.On = false;
        }
    }

    public void SetOrAdd(string key, bool value) {
        if (conditions.ContainsKey(key))
        {
            conditions[key] = value;
        }
        else {
            conditions.Add(key, value);
        }
    }

}
