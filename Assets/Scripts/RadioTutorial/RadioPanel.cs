using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RadioPanel : MonoBehaviour {
    public static RadioPanel Current;
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

    public ScrollingText scroller;
    public MyUIAnimation radio;

    public GameObject LeftButton;
    public GameObject RightButton;

    public GameObject RadioActions;

    public RadioObject current;

    private void Awake()
    {
        Current = this;
    }

    private void Start()
    {
        List<RadioObject> objs = new List<RadioObject>();
        for (int i = 0; i < RadioActions.transform.childCount; i++) {
            objs.Add(RadioActions.transform.GetChild(i).GetComponent<RadioObject>());
        }

        for (int i = 0; i < objs.Count-1; i++) {
            objs[i].defaultNext = objs[i + 1];
        }

        for (int i = 1; i < objs.Count; i++) {
            objs[i].last = objs[i - 1];
        }

        current = objs[0];
        scroller.LoadText(current.text);
        scroller.Display();
        scroller.fastforward();
    }

    public void proceed(string code = "") {

        if (code != "") {
            current = current.GetNext(code);
            scroller.LoadText(current.text);
            scroller.Display();
            scroller.fastforward();
            return;
        }


        if (scroller.isDone)
        {
            if (RadioPanel.check(current.conditions))
            {
                current = current.GetNext();
                scroller.LoadText(current.text);
                scroller.Display();
                scroller.fastforward();
            }
        }
        else {
            scroller.fastforward();
        }
    }

    public void reverse() {
        if (current.last == null)
        {
            return;
        }
        else {
            current = current.last;
            scroller.LoadText(current.text);
            scroller.Display();
            scroller.fastforward();
        }
    }

    public void Update()
    {
        if (current.defaultNext == null || !check(current.conditions))
        {
            RightButton.SetActive(false);
        }
        else if (current.defaultNext != null && check(current.conditions)) {
            RightButton.SetActive(true);
        }

        if (current.last == null)
        {
            LeftButton.SetActive(false);
        }
        else {
            LeftButton.SetActive(true);
        }
    }
}
