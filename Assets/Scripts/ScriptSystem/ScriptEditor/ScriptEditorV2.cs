using System.Collections;
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
    
    public ScriptSystem ScriptSystemObject {
        get; private set;
    }
    
    void Awake() {
        this.DiscardButtonObject.onClick.AddListener(this.DiscardEdit);
        this.SaveButtonObject.onClick.AddListener(this.SaveAndRun);
        
        this.gameObject.SetActive(false);
    }

    void Start() {
        
    }
    
    public void StartEdit(ScriptSystem scriptSystem) {
        if (this.ScriptSystemObject != null) {
            this.DiscardEdit();
        }
        
        this.gameObject.SetActive(true);
        MyMono.Paused = true;
        
        this.ScriptSystemObject = scriptSystem;
        this.APIPanelObject.LoadBlockDefs(this.ScriptSystemObject.GetBlockDefs());
        
        this.ScriptPanelObject.LoadString(this.ScriptSystemObject.BlockScript);
        
        this.ReloadStatus();
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
        }
        else {
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
        MyMono.Paused = false;
        
        this.ScriptSystemObject = null;
    }
    
    public override void NormalUpdate() {
        
    }

    private void OnEnable()
    {
        Inventory.Current.gameObject.SetActive(false);
        ProductionUI.Current.gameObject.SetActive(false);
        ResourceDisplay.Current.gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        // Inventory.Current.gameObject.SetActive(true);
        // ResourceDisplay.Current.gameObject.SetActive(true);
    }

}
