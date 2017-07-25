using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;


public class TextDisplayComponent : UnitComponent {
    protected double defaultDisplayTime = 5.0;
    
    private double timer = 0;
    
    public Text Text;
    
    void Awake() {
        this.gameObject.SetActive(false);
    }
    
    public override void PausingFixedUpdate() {
        base.PausingFixedUpdate();
        if (this.timer <= 0) return;
        
        this.timer -= Time.deltaTime;
        if (this.timer <= 0) {
            this.StopDisplay();
        }
    }
    
    public void Display(string text) {
        this.DisplayFor(text, this.defaultDisplayTime);
    }

    public void DisplayFor(string text, double duration) {
        if (duration < 0.01) duration = 0.01;
        this.timer = duration;
        this.Text.text = text;
        this.gameObject.SetActive(true);
    }
    
    public void StopDisplay() {
        this.timer = 0;
        this.gameObject.SetActive(false);
    }

    public override void Register(ScriptSystem scriptSystem) {
        scriptSystem.RegisterFunction("display", new Action<string>(this.Display));
        scriptSystem.RegisterFunction("display_for", new Action<string, double>(this.DisplayFor));
        scriptSystem.RegisterFunction("stop_display", new Action(this.StopDisplay));
    }
}
