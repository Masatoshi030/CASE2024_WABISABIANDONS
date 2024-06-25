using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyA_WaitPlayer : EnemyState
{
    [SerializeField, Header("プレイヤーの通過検知があったか")]
    public bool isDetection = false;

    [SerializeField, Header("移動中")]
    bool isMove = false;

    [SerializeField, Header("検知時の移動方向")]
    Transform detectionTransform;

    [SerializeField, Header("検知時の移動速度")]
    float velocitySpeed;

    [Space(pad), Header("--遷移先リスト--")]
    [SerializeField, Header("検知後の最終遷移先")]
    int finalTransitionID;

    [SerializeField, Header("被弾時の遷移先")]
    int damagedID;

    public override void Enter()
    {
        base.Enter();
        enemy.IsVelocityZero = false;
    }

    public override void MainFunc()
    {
        base.MainFunc();
        if(enemy.IsFindPlayer && !isDetection)
        {
            isMove = true;
            enemy.EnemyRigidbody.velocity = detectionTransform.forward * velocitySpeed;
            isDetection = true;
        }
        else if(isDetection && !isMove)
        {
            isMove = true;
            enemy.EnemyRigidbody.velocity = detectionTransform.forward * velocitySpeed;
        }
        else if(enemy.IsDamaged)
        {
            machine.TransitionTo(damagedID);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void CollisionEnterSelf(Collision collision)
    {
        base.CollisionEnterSelf(collision);
        if (isDetection)
        {
            if (collision.transform.tag == "Ground")
            {
                machine.TransitionTo(finalTransitionID);
            }
        }
    }
}
