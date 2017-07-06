using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dialogue : MonoBehaviour {
    
    public string text;
    public Dialogue next;
    public List<string> conditions;

    public virtual void PreDisplay() {

    }

    public virtual void PostDisplay()
    {

    }

    public void Run(){
        PreDisplay();
        PostDisplay();
    }

}
