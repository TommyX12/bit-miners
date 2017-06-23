using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour {

    public bool ShouldRun = true;
    public float time;
    public int SceneId;

    float timer = 0;

    private void Start()
    {
        timer = time;
    }

    void Update () {
        timer -= Time.deltaTime;
        if (timer <=0) {
            SceneManager.LoadScene(SceneId);
        }
	}
}
