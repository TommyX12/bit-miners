using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionSystem : MonoBehaviour
{

    public static ActionSystem Current;
    public static Dictionary<string, bool> Conditions = new Dictionary<string, bool>();

    public static bool check(List<string> bools)
    {
        foreach (string s in bools)
        {
            if (Conditions.ContainsKey(s))
            {
                if (Conditions[s])
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

    public static void SetOrAdd(string key, bool value)
    {
        if (Conditions.ContainsKey(key))
        {
            Conditions[key] = value;
        }
        else
        {
            Conditions.Add(key, value);
        }
    }

    // -------------------------------- \\

    public LevelAction currentAction;



    public void proceed()
    {
        if (currentAction.NextAction != null)
        {
            currentAction = currentAction.NextAction;
            currentAction.run();
        }
    }

    private void Start()
    {
        Current = this;

        // reset bools. Or they won't be reset after reloading scene.
        Conditions.Clear();

        List<LevelMultiAction> multiactions = new List<LevelMultiAction>();
        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject child = transform.GetChild(i).gameObject;
            LevelAction[] actions = child.GetComponents<LevelAction>();
            LevelMultiAction multiAct = child.AddComponent<LevelMultiAction>();
            multiAct.actions = new List<LevelAction>(actions);
            multiactions.Add(multiAct);
        }
        for (int i = 0; i < multiactions.Count; i++)
        {
            if (i != multiactions.Count - 1)
            {
                multiactions[i].NextAction = multiactions[i + 1];
            }
        }
        currentAction = multiactions[0];
        if(currentAction!=null)
        currentAction.run();
    }


    private void Update()
    {
        if (currentAction.isDone)
        {
            proceed();
        }
    }
}
