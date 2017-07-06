using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WASDCircle : MonoBehaviour {

    public WASDCircle next;
    public static int count = 4;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        gameObject.SetActive(false);

        count--;
        if (count <= 0)
        {
            if (TutorialSystem.Current.conditions.ContainsKey("WASDDone"))
            {
                TutorialSystem.Current.conditions["WASDDone"] = true;
            }
            else {
                TutorialSystem.Current.conditions.Add("WASDDone", true);
            }
            TutorialSystem.Current.next();
            Destroy(gameObject, 1);
            return;
        }

        Destroy(gameObject, 1);
        next.gameObject.SetActive(true);
    }

}
