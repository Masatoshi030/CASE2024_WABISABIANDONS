using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageManager : MonoBehaviour
{
    public static MessageManager instance;

    MessageElementController currentMessage = null;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetCurrentMessage(MessageElementController message)
    {
        if(currentMessage != null)
        {
            currentMessage.SetInactiveTutorial();
            currentMessage = message;
        }
        else
        {
            currentMessage = message;
        }
    }

    public void ClearElement()
    {
        currentMessage = null;
    }
}
