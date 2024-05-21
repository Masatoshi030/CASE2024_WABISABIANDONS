using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G_ValveRotate : GimmickState
{
    [SerializeField, Header("回転させるオブジェクト")]
    GameObject rotateObj;
    [SerializeField, Header("移動させるオブジェクト")]
    GameObject moveObj;
    [SerializeField, Header("回転軸"), VectorRange(-1.0f, 1.0f)]
    Vector3 rotateAxis;
    [SerializeField, Header("初期速度")]
    float initSpeed;
    [SerializeField, Header("作動時間")]
    float runningTime;
    [SerializeField, Header("秒間減速値"), ReadOnly]
    float brakeValue;
    [SerializeField, Header("現在速度"), ReadOnly]
    float currentSpeed;
    [SerializeField, Header("移動するオブジェクトの初期位置"), ReadOnly]
    Vector3 initObjectPosition;
    [SerializeField, Header("移動方向"), VectorRange(-1.0f, 1.0f)]
    Vector3 moveVelocity;
    [SerializeField, Header("オブジェクトの移動値")]
    float moveValue;
    [SerializeField, Header("移動→停止までにかかる時間")]
    float initTime;

    [Space(pad), Header("--遷移先リスト--")]
    [SerializeField, Header("時間経過後の遷移")]
    string elapsedTransition;

    public override void Enter()
    {
        base.Enter();
        currentSpeed = initSpeed;
        brakeValue = initSpeed / runningTime;
    }

    public override void MainFunc()
    {
        if(currentSpeed <= 0)
        {
            machine.TransitionTo(elapsedTransition);
        }
        rotateObj.transform.Rotate(rotateAxis, currentSpeed * Time.deltaTime);
        currentSpeed = currentSpeed - brakeValue * Time.deltaTime;
    }
}
