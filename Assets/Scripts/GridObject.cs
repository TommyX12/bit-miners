using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridObject : MonoBehaviour {

    private int x;
    private int y;
    private int facing;

    private void Start()
    {
        int[] pos = worldToGridSpace(transform.position);
        x = pos[0];
        y = pos[1];
        doTranslate(x, y);
    }

    public int[] getPos() {
        return new int[] { x, y };
    }

    public void translate(int x, int y) {
        this.x += x;
        this.y += y;
        doTranslate(this.x, this.y);
    }

    private void doTranslate(int x, int y) {
        Vector2 v = gridToWorldSpace(x, y);
        transform.position = new Vector3(v.x, v.y, transform.position.z);
    }

    public static Vector2 gridToWorldSpace(int x, int y) {
        return new Vector2(x * 1 + 0.5f, y * 1 + 0.5f);
    }

    public static int[] worldToGridSpace(Vector2 pos) {
        return new int[] { (int)(pos.x - 0.5), (int)(pos.y - 0.5) };
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W)) {
            this.translate(0, 1);
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            this.translate(-1, 0);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            this.translate(0,-1);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            this.translate(1, 0);
        }
    }
}
