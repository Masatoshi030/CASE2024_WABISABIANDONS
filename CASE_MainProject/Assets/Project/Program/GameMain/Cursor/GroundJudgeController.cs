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

    [SerializeField, Header("接地判定"),
        Toolbar(typeof(ON_GROUND_STATE), "OnGroundState")]
    public ON_GROUND_STATE onGroundState = ON_GROUND_STATE.Off;

    private void Awake()
    {
        onGroundState = ON_GROUND_STATE.Off;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Ground")
        {
            onGroundState = ON_GROUND_STATE.On;

            //カーソルを接地判定
            CursorController.instance.ChangeCursorState(CursorController.ON_CURSOR_STATE.Idle);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Ground")
        {
            onGroundState = ON_GROUND_STATE.On;

            //カーソルを接地判定
            CursorController.instance.ChangeCursorState(CursorController.ON_CURSOR_STATE.Idle);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Ground")
        {
            onGroundState = ON_GROUND_STATE.Off;
            //カーソルを非接地判定
            CursorController.instance.ChangeCursorState(CursorController.ON_CURSOR_STATE.OffGround);
        }
    }
}
