using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;


public class TextDisplayComponent : UnitComponent {
    protected double defaultDisplayTime = 5.0;
    
    private double timer = 0;
    
    public Text Text;
    public Canvas Canvas;
    
    private bool errorFound = false;
    
    void Awake() {
        this.Hide();
    }
    
    protected void Hide() {
        this.Canvas.gameObject.SetActive(false);
    }
    
    protected void Show() {
        this.Canvas.gameObject.SetActive(true);
    }
    
    public override void PausingUpdate() {
        base.PausingUpdate();
        
        if (this.unit != null && this.unit.ScriptSystemObject.ErrorCaught) {
            if (!this.errorFound) {
                this.DisplayError(this.unit.ScriptSystemObject.Message);
                this.errorFound = true;
            }
        }
        
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
        this.Text.color = Color.black;
        this.Show();
    }
    
    public void DisplayError(string text) {
        this.timer = -1;
        this.Text.text = text;
        this.Text.color = Color.red;
        this.Show();
    }
    
    public void StopDisplay() {
        this.timer = 0;
        this.Hide();
    }

    public override void Register(ScriptSystem scriptSystem) {
        scriptSystem.RegisterFunction("display", new Action<string>(this.Display));
        scriptSystem.RegisterFunction("display_for", new Action<string, double>(this.DisplayFor));
        scriptSystem.RegisterFunction("stop_display", new Action(this.StopDisplay));
    }
    
    public override void PreRun(ScriptSystem scriptSystem) {
        this.StopDisplay();
        this.errorFound = false;
    }
}
