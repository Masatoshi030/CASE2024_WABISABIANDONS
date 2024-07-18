using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundJudgeController : MonoBehaviour
{

    public enum ON_GROUND_STATE
    {
        NewOn,
        On,
        NewOff,
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
        if (other.tag == "Ground" ||
            (other.tag == "BrokenWall" && PlayerController.instance.attackState != PlayerController.ATTACK_STATE.Attack) ||
            (other.tag == "BreakBox" && PlayerController.instance.attackState != PlayerController.ATTACK_STATE.Attack))
        {
            onGroundState = ON_GROUND_STATE.NewOn;

            //プレイヤーの着地したときの処理を呼ぶ
            PlayerController.instance.NewOnGround();

            //カーソルを接地判定
            CursorController.instance.ChangeCursorState(CursorController.ON_CURSOR_STATE.Idle);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Ground" || 
            (other.tag == "BrokenWall" && PlayerController.instance.attackState != PlayerController.ATTACK_STATE.Attack)||
            (other.tag == "BreakBox" && PlayerController.instance.attackState != PlayerController.ATTACK_STATE.Attack))
        {
            onGroundState = ON_GROUND_STATE.On;

            //プレイヤーの着地しているときの処理を呼ぶ
            PlayerController.instance.StayOnGround();

            //カーソルを接地判定
            CursorController.instance.ChangeCursorState(CursorController.ON_CURSOR_STATE.Idle);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Ground" ||
            (other.tag == "BrokenWall" && PlayerController.instance.attackState != PlayerController.ATTACK_STATE.Attack) ||
            (other.tag == "BreakBox" && PlayerController.instance.attackState != PlayerController.ATTACK_STATE.Attack))
        {
            onGroundState = ON_GROUND_STATE.NewOff;

            //プレイヤーの離地したときの処理を呼ぶ
            PlayerController.instance.NewExitGround();

            //カーソルを非接地判定
            CursorController.instance.ChangeCursorState(CursorController.ON_CURSOR_STATE.OffGround);
        }
    }

    private void Update()
    {
        //StayExit
        if (onGroundState == ON_GROUND_STATE.NewOff)
        {
            onGroundState = ON_GROUND_STATE.Off;
        }
        else if (onGroundState == ON_GROUND_STATE.NewOff)
        {
            //プレイヤーの離地している間の処理を呼ぶ
            PlayerController.instance.StayExitGround();
        }
    }
}
