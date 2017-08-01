using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SendMessageCommand : LevelAction {
    public GameObject ToSendMessage;
    public string message;
    public override void run()
    {
        base.run();
        ToSendMessage.SendMessage(message,SendMessageOptions.DontRequireReceiver);

        running = false;
        isDone = true;
    }

}
