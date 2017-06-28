using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour {

    public GameObject Menu;
    
    bool isOn = false;

    void TurnOn() {
        Menu.SetActive(true);
        isOn = true;
    }

    void TurnOff() {
        Menu.SetActive(false);
        isOn = false;
    }

    public void Restart() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Quit() {
        SceneManager.LoadScene(0);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (isOn)
            {
                TurnOff();
            }
            else {
                TurnOn();
            }
        }
    }
}
