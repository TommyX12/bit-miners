using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadioObject : MonoBehaviour {

    public List<string> conditions;

    public List<string> codes;
    public List<RadioObject> altNext;

    public Dictionary<string, RadioObject> lookup = new Dictionary<string,RadioObject>();

    public RadioObject defaultNext;
    public RadioObject last;
    public string text;

    private void Start()
    {
        for (int i = 0; i < codes.Count; i++) {
            lookup.Add(codes[i], altNext[i]);
        }
        if (!codes.Contains("fail")) {
            lookup.Add("fail", this);
        }
    }

    public virtual RadioObject GetNext(string code = null) {
        if (code == null)
        {
            return defaultNext;
        }
        else {
            return lookup[code];
        }
    }
}
