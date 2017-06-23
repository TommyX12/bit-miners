using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ExampleCode : MonoBehaviour {
    public static ExampleCode Current;

    public List<TextAsset> sources;
    private List<string> exampleCodes;

    private void Start()
    {
        Current = this;
        exampleCodes = new List<string>();
        foreach (TextAsset ta in sources) {
            exampleCodes.Add(ta.text);
        }
    }

    public string GetRandomCode() {
        return Util.GetRandomElement<string>(exampleCodes);
    }
}
