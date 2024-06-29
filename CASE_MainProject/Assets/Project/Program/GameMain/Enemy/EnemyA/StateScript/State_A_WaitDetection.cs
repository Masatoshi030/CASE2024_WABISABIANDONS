using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 検知用オブジェクトからの命令発行待ち状態
public class State_A_WaitDetection : EnemyState
{
    public bool isDetection = false;
    bool isMove = false;

    [SerializeField, Header("検知時の移動方向")]
    Transform detectionTransform;

    [SerializeField, Header("検知時の移動速度")]
    float velocitySpeed;

    [Space(pad), Header("--遷移先リスト--")]
    [SerializeField, Header("検知後の最終遷移")]
    StateKey detectedKey;

    [SerializeField, Header("ダメージ時の遷移")]
    StateKey damagedKey;

    public override void Enter()
    {
        base.Enter();
        enemy.IsVelocityZero = false;
    }

    public override void MainFunc()
    {
        base.MainFunc();
        
        // 検知オブジェクトからの通知なしに自身でプレイヤーを発見
        if(enemy.IsFindPlayer && !isDetection)
        {
            isMove = true;
            enemy.EnemyRigidbody.velocity = detectionTransform.forward * velocitySpeed;
            isDetection = true;
        }
        // 検知オブジェクトからの通知で発見
        else if(isDetection && !isMove)
        {
            isMove = true;
            enemy.EnemyRigidbody.velocity = detectionTransform.forward * velocitySpeed;
        }
        // 被弾時の遷移
        else if(enemy.IsDamaged)
        {
            machine.TransitionTo(damagedKey);
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
                Enemy_Manager.instance.CreateSteamEffect(enemy.transform.position, Quaternion.identity);
                machine.TransitionTo(detectedKey);
            }
        }
    }
}
