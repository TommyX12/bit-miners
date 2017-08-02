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
        if (MyMono.Paused || this.isOn) return;
        Menu.SetActive(true);
        MyMono.Paused = true;
        this.isOn = true;
    }

    public void TurnOff() {
        Menu.SetActive(false);
        MyMono.Paused = false;
        this.isOn = false;
    }

    public void Restart() {
        MyMono.Paused = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Quit() {
        SceneManager.LoadScene(0);
    }

    private void Update()
    {
        if (MyMono.Paused) {
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
