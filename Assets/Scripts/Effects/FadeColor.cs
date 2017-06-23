using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class FadeColor : MonoBehaviour {
    public Color From;
    public Color To;
    public Image target;

    public float time;
    private float timer;

    private void FixedUpdate()
    {
        float dr = (To.r - From.r)*(timer/time);
        float dg = (To.g - From.g) * (timer / time);
        float db = (To.b - From.b) * (timer / time);
        float da = (To.a - From.a) * (timer / time);

        target.color = new Color(From.r + dr, From.g + dg, From.b + db, From.a + da);
        timer += Time.deltaTime;
    }
}
