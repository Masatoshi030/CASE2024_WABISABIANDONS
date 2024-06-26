using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class Detection : Publisher
{
    [SerializeField, Header("Renderer")]
    Renderer myRenderer;

    private void Awake()
    {
        myRenderer.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            for(int i = 0; i < subscribers.Count; i++)
            {
                if (subscribers[i] != null)
                {
                    Enemy enemy = (Enemy)subscribers[i];
                    if(enemy.Machine.StateObject.GetComponent<State_A_WaitDetection>() != null)
                    {
                        State_A_WaitDetection state = enemy.Machine.StateObject.GetComponent<State_A_WaitDetection>();
                        state.isDetection = true;
                    }
                }
            }

            Destroy(gameObject);
        }

        
    }
}
