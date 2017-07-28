using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionSystem : MonoBehaviour
{

    public static ActionSystem Current;

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
    }

    private void Update()
    {
        if (currentAction.isDone)
        {
            proceed();
        }
    }
}
