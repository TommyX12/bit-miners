using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetBlockFilter : MonoBehaviour {
    public List<string> types;
    public ScriptEditorV2 editor;

    private void Start()
    {
        editor.SetBlockTypeFilter(types);
    }
}
