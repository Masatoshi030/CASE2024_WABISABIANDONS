using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_A_RunAway : EnemyState
{
    [SerializeField, Header("移動速度")]
    float moveSpeed;
    [SerializeField, Header("加速度")]
    float accelerationSpeed;
    [SerializeField, Header("回転速度")]
    float angularSpeed;

    NavMeshPatrol patrol;

    [Space(pad), Header("--遷移先リスト--")]
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

        // 逃亡方向計算
        Vector3 Direction = (Enemy.Target.transform.position - enemy.transform.position).normalized;
        Direction = -Direction;

        patrol.ExcuteCustom(enemy.transform.position + Direction * 3.0f);
    }

    public override void MainFunc()
    {
        base.MainFunc();

        // 逃亡方向計算
        Vector3 Direction = (Enemy.Target.transform.position - enemy.transform.position).normalized;
        Direction = -Direction;

        patrol.ExcuteCustom(enemy.transform.position + Direction * 3.0f);

        if(enemy.IsDamaged)
        {
            PlayerController.instance.OnHitStop(1.0f, 0.1f);
            machine.TransitionTo(damagedKey);
        }
    }
}
