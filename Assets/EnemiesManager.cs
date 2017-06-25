using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesManager : MonoBehaviour {
    public static EnemiesManager Current;
    public List<GameObject> enemySource;
    public Dictionary<string, GameObject> Enemies;

    private void Awake()
    {
        Current = this;
        foreach (GameObject obj in enemySource) {
            Enemies.Add(obj.name, obj);
        }
    }
}
