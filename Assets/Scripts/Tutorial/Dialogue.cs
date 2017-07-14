using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dialogue : MonoBehaviour {
    
    public string text;
    public Dialogue next;
    public Dialogue last;
    public List<string> conditions;
    public Sprite portrait;
    public AudioClip sound;
    bool run = false;

    public virtual void PreDisplay() {

    }

    public virtual void PostDisplay()
    {

    }

    public void Run(){
        if (!run)
        {
            PreDisplay();
            PostDisplay();
        }
        run = false;
    }
}
