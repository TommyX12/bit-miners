using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRangeEnable : MonoBehaviour {

    public GameObject ToEnable;
    public Collider2D col;

    private void Start()
    {
        if (col == null) {
            col = GetComponent<Collider2D>();
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) {
            ToEnable.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) {
            ToEnable.SetActive(false);
        }
    }

}
