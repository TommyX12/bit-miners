using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResizingButtonPanel : MyMono {

    public int initialXpadding;
    public int initialYpadding;
    public int ElementWidth;
    public int interpadding;
    public GameObject ButtonPrefab;
    public RectTransform rt;

    public List<GameObject> buttons = new List<GameObject> ();

    private void Start()
    {
        rt = GetComponent<RectTransform>();
        gameObject.SetActive(false);
    }


    public void Refresh(List<GameObject> prefabItems) {
        int UIPosition_x = 0;
        int UIPosition_y = 0;
        int elementCounter = 0;

        UIPosition_x += initialXpadding;
        UIPosition_y += initialYpadding;

        foreach (GameObject b in buttons) {
            Destroy(b);
        }

        foreach (GameObject go in prefabItems) {
            GameObject Button = GameObject.Instantiate(ButtonPrefab);
            RectTransform buttonrt = Button.GetComponent<RectTransform>();
            UICapsule capsule = Button.GetComponentInChildren<UICapsule>();
            capsule.index = elementCounter;
            capsule.image.sprite = BuildUI.Current.ActiveList[elementCounter].GetComponentInChildren<SpriteRenderer>().sprite;
            buttonrt.SetParent(rt);
            buttonrt.localPosition = new Vector2(UIPosition_x, -UIPosition_y);
            buttons.Add(Button);
            UIPosition_x += interpadding + ElementWidth;
            elementCounter++;
        }

        UIPosition_x += initialXpadding;
        UIPosition_y += initialYpadding;
        rt.sizeDelta = new Vector2(UIPosition_x, rt.sizeDelta.y);
    }
}
