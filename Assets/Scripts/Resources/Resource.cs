using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : MyMono {

    public int MaxResourceValue;
    public int ResourceValue;
    public int ResourcePerCollection;
    public float CollectionTime;

    public int GetResource() {
        int value = ResourcePerCollection;
        ResourceValue -= ResourcePerCollection;
        if (ResourceValue <= 0) {
            value += ResourceValue;
            gameObject.SetActive(false);
        }
        return value;
    }

}
