using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialSystem : MyMono {

    public Dictionary<string, bool> conditions;
    public DialogueDisplay display;

    public Dialogue current;
    public Dialogue start;

    public void next() {
        if (check(current.conditions))
        {
            current = current.next;
            current.Run();
            display.Show(current.text);
        }
        else{
            display.Show(current.text);
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
}
