using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemy : MyMono {
    public float time;
    float timer;
    public GameObject enemyPrefab;
    private void Start()
    {
        timer = time;
    }

    public override void PausingFixedUpdate()
    {
        base.PausingFixedUpdate();
        if (timer <= 0) {
            GameObject.Instantiate(enemyPrefab).transform.position = transform.position;
            timer = time;
        }
        timer -= Time.deltaTime;
    }
}
