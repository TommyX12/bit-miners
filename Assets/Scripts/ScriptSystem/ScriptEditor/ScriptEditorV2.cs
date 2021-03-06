﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScriptEditorV2 : MyMono {
    
    public static readonly Color ERROR_COLOR = new Color(0.75f, 0.2f, 0.0f, 1.0f);
    public static readonly Color IDLE_COLOR = new Color(0.0f, 0.0f, 0.2f, 1.0f);
    public static readonly Color RUNNING_COLOR = new Color(0.0f, 0.5f, 0.1f, 1.0f);
    
    public Text StatusTextObject;
    public APIPanel APIPanelObject;
    public ScriptPanel ScriptPanelObject;
    public Button SaveButtonObject;
    public Button DiscardButtonObject;
    
    private List<string> blockTypeFilter = null;
    
    public ScriptSystem ScriptSystemObject {
        get; private set;
    }
    
    void Awake() {
        this.DiscardButtonObject.onClick.AddListener(this.DiscardEdit);
        this.SaveButtonObject.onClick.AddListener(this.SaveAndRun);
    }

    void Start() {
        this.gameObject.SetActive(false);
    }
    
    public void StartEdit(ScriptSystem scriptSystem) {
        if (this.ScriptSystemObject != null) {
            this.DiscardEdit();
        }
        
        this.gameObject.SetActive(true);
        GameManager.Current.Paused = true;
        
        this.ScriptSystemObject = scriptSystem;
        this.APIPanelObject.LoadBlockDefs(this.ScriptSystemObject.GetBlockDefs(), this.blockTypeFilter);
        
        this.ScriptPanelObject.LoadString(this.ScriptSystemObject.BlockScript);
        
        this.ReloadStatus();
        
        MusicManager.Current.SetCondition("editor_open", true);
    }
    
    // set to null for no filter
    public void SetBlockTypeFilter(List<string> filter) {
        this.blockTypeFilter = filter;
    }
    
    public void DiscardEdit() {
        this.EndEdit();
    }
    
    public void SaveAndRun() {
        List<List<SEElementDef>> defList = this.ScriptPanelObject.GetDefList();
        this.ScriptSystemObject.BlockScript = ScriptPanel.SaveListToString(defList);
        this.ScriptSystemObject.Script = ScriptPanel.Compile(defList, this.APIPanelObject.GetCompileFunc);
        // Debug.Log(this.ScriptSystemObject.Script);
        this.ScriptSystemObject.Start();
        
        if (!this.ScriptSystemObject.ErrorCaught) this.EndEdit();
        else this.ReloadStatus();
    }
    
    private void ReloadStatus() {
        if (this.ScriptSystemObject.ErrorCaught) {
            this.StatusTextObject.color = ERROR_COLOR;
            if (this.ScriptSystemObject.ErrorLine > 0) this.ScriptPanelObject.SetErrorLine(this.ScriptSystemObject.ErrorLine - 1);
        }
        else {
            this.ScriptPanelObject.ClearErrorLine();
            if (this.ScriptSystemObject.Running) {
                this.StatusTextObject.color = RUNNING_COLOR;
            }
            else {
                this.StatusTextObject.color = IDLE_COLOR;
            }
        }
        
        this.StatusTextObject.text = this.ScriptSystemObject.Message;
    }
    
    private void EndEdit() {
        this.gameObject.SetActive(false);
        GameManager.Current.Paused = false;
        
        this.ScriptSystemObject = null;
        
        MusicManager.Current.SetCondition("editor_open", false);
    }
    
    public override void NormalUpdate() {
        
    }

    private void OnEnable()
    {
        // Inventory.Current.gameObject.SetActive(false);
        // ProductionUI.Current.gameObject.SetActive(false);
        // ResourceDisplay.Current.gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        // Inventory.Current.gameObject.SetActive(true);
        // ResourceDisplay.Current.gameObject.SetActive(true);
    }

}
