using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_A_Tracking : EnemyState
{
    [SerializeField, Header("移動速度")]
    float moveSpeed;
    [SerializeField, Header("加速度")]
    float accelerationSpeed;
    [SerializeField, Header("回転速度")]
    float angularSpeed;
    [SerializeField, Header("追跡時間")]
    float trackingTime = 3.0f;
    [SerializeField, Header("追跡成功距離")]
    float trackSuccessDistance = 7.0f;

    NavMeshPatrol patrol;
    float subCnt = 0.0f;
    bool allowActive = false;

    [Space(pad), Header("--遷移先リスト--")]
    [SerializeField, Header("距離一定以内の遷移")]
    StateKey successedKey;
    [SerializeField, Header("経過時の遷移")]
    StateKey elapsedKey;
    [SerializeField, Header("ダメージ時のKey")]
    StateKey damagedKey;

    public override void Initialize()
    {
        base.Initialize();

        // パトロール用コンポーネントを取得
        patrol = enemy.GetComponent<NavMeshPatrol>();
    }

    public override void Enter()
    {
        base.Enter();

        // エージェント自体のenabled
        patrol.Agent.enabled = true;
        patrol.enabled = true;
        patrol.Agent.velocity = Vector3.zero;
        // パラメータの設定
        patrol.SetAgentParam(moveSpeed, accelerationSpeed, angularSpeed);
        // パスの設定
        patrol.ExcuteCustom(Enemy.Target.transform.position);
        enemy.IsVelocityZero = true;
        subCnt = 0.0f;
        allowActive = true;
        enemy.SetAllowActive(allowActive);
    }

    public override void MainFunc()
    {
        base.MainFunc();
        enemy.AllowObject.GetComponent<TargetAllow>().StartDesignation(enemy.EyeTransform.position, Enemy.Target.transform.position);
        subCnt += Time.deltaTime;
        if(subCnt >= 1.0f)
        {
            // 1秒毎にパスの設定
            patrol.ExcuteCustom(Enemy.Target.transform.position);
            subCnt = 0.0f;
            // 矢印表示の反転
            allowActive = !allowActive;
            enemy.SetAllowActive(allowActive);
        }
        

        if(enemy.IsDamaged)
        {
            machine.TransitionTo(damagedKey);
        }
        else if(machine.Cnt >= trackingTime)
        {
            machine.TransitionTo(elapsedKey);
        }
        else if(enemy.ToPlayerDistace <= (trackSuccessDistance * trackSuccessDistance))
        {
            machine.TransitionTo(successedKey);
        }
    }

    public override void Exit()
    {
        // 矢印表示の削除
        enemy.AllowObject.GetComponent<TargetAllow>().EndDesignation();
        enemy.AllowObject.SetActive(false);
        patrol.Stop();
        patrol.Agent.enabled = false;
    }
}
