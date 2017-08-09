using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour {

    public GameObject Menu;
    public GameObject Hint;
    
    bool isOn;
    
    void Awake() {
        this.isOn = false;
        Menu.SetActive(false);
    }
    
    public void TurnOn() {
        if (GameManager.Current.Paused || this.isOn) return;
        Menu.SetActive(true);
        GameManager.Current.Paused = true;
        this.isOn = true;
    }

    public void TurnOff() {
        Menu.SetActive(false);
        GameManager.Current.Paused = false;
        this.isOn = false;
    }

    public void Restart() {
        GameManager.Current.Paused = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Quit() {
        GameManager.Current.Paused = false;
        SceneManager.LoadScene(0);
    }

    private void Update()
    {
        MusicManager.Current.SetCondition("pause_menu_open", Menu.activeSelf);
        if (GameManager.Current.Paused) {
            if (this.Hint.activeSelf) {
                this.Hint.SetActive(false);
            }
        }
        else {
            if (!this.Hint.activeSelf) {
                this.Hint.SetActive(true);
            }
        }
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (this.isOn) {
                TurnOff();
            }
            else {
                TurnOn();
            }
        }
    }
}
