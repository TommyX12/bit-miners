using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class NextScenePanel : MonoBehaviour {
    public int scene_id;

    public void Next() {
        SceneManager.LoadScene(scene_id);
    }
}
