using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomRadiusSpawn : MonoBehaviour {
    public GameObject prefab;

    public float radius;

    public int count;

    private void Start()
    {
        for (int i = 0; i < count; i++)
        {
            GameObject spawned = GameObject.Instantiate(prefab);
            spawned.GetComponent<SpriteRenderer>().color = new Color(Random.Range(0, 255)/255f, Random.Range(0, 255) / 255f, Random.Range(0, 255) / 255f);
            spawned.transform.position = transform.position + (Vector3) new Vector2(Random.Range(-radius, radius), Random.Range(-radius, radius));
        }
    }

}
