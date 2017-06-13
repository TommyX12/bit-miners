using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowScript : MonoBehaviour {
    public GameObject target;

    private void LateUpdate()
    {
        gameObject.transform.position = new Vector3(target.transform.position.x, target.transform.position.y, gameObject.transform.position.z);
    }
}
