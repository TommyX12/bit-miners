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

    public void Start()
    {
        conditions = new Dictionary<string, bool>();
        Current = this;
        start.Run();
        display.Show(start.text);

    }

    public void next() {
        if (check(active.conditions))
        {
            active = active.next;
            active.Run();
            display.Show(active.text);
        }
        else{
            display.Show(active.text);
        }
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
