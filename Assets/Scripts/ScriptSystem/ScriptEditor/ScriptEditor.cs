using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScriptEditor : MyMono {
	
	public static readonly Color ERROR_COLOR = new Color(0.75f, 0.2f, 0.0f, 1.0f);
	public static readonly Color IDLE_COLOR = new Color(0.0f, 0.0f, 0.2f, 1.0f);
	public static readonly Color RUNNING_COLOR = new Color(0.0f, 0.5f, 0.1f, 1.0f);
	
	public InputField InputFieldObject;
	public Text StatusTextObject;
	public Text APITextObject;
	public Button SaveButtonObject;
	public Button DiscardButtonObject;
	
	public ScriptSystem ScriptSystemObject {
		get; private set;
	}
	
	void Awake() {
		this.DiscardButtonObject.onClick.AddListener(this.DiscardEdit);
		this.SaveButtonObject.onClick.AddListener(this.SaveAndRun);
	}

	void Start() {
		
	}
	
	public void StartEdit(ScriptSystem scriptSystem) {
		if (this.ScriptSystemObject != null) {
			this.DiscardEdit();
		}
		
		this.gameObject.SetActive(true);
		GameManager.Current.Paused = true;
		
		this.ScriptSystemObject = scriptSystem;
		
		this.InputFieldObject.text = this.ScriptSystemObject.Script;
		this.APITextObject.text = this.ScriptSystemObject.GetAPIListText();
		
		this.ReloadStatus();
	}
	
	public void DiscardEdit() {
		this.EndEdit();
	}
	
	public void SaveAndRun() {
		this.ScriptSystemObject.Script = this.InputFieldObject.text;
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
		GameManager.Current.Paused = false;
		
		this.ScriptSystemObject = null;
		
		InputFieldObject.text = "";
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
        Inventory.Current.gameObject.SetActive(true);
        ResourceDisplay.Current.gameObject.SetActive(true);
    }

}
