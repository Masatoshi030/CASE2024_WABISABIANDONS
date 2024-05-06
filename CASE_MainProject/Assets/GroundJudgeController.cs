using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundJudgeController : MonoBehaviour
{

    public enum ON_GROUND_STATE
    {
        On,
        Off
    }

    [SerializeField, Header("ê⁄ínîªíË"),
        Toolbar(typeof(ON_GROUND_STATE), "OnGroundState")]
    public ON_GROUND_STATE onGroundState = ON_GROUND_STATE.On;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Ground")
        {
            onGroundState = ON_GROUND_STATE.On;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Ground")
        {
            onGroundState = ON_GROUND_STATE.Off;
        }
    }
}
