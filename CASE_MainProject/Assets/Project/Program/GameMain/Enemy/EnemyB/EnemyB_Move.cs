using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 上下に移動する
public class EnemyB_Move : EnemyState
{
    [SerializeField, Header("初期位置"), ReadOnly]
    Vector3 initPos;
    [SerializeField, Header("移動時間")]
    float moveTime;
    float moveAmount;
    [SerializeField, Header("移動速度")]
    float moveSpeed;
    [SerializeField, Header("初期移動方向が上")]
    bool initIsUp = true;
    bool nowIsUp;

    [Space(pad), Header("遷移先リスト")]
    [SerializeField, Header("索敵成功時の遷移")]
    public int searchSuccessID;
    [SerializeField, Header("時間経過後の遷移")]
    public int elapsedID;
    [SerializeField, Header("被弾時の遷移")]
    public int damagedID;

    public override void Initialize()
    {
        initPos = enemy.transform.position;
        nowIsUp = initIsUp;
    }

    public override void Enter()
    {

    }

    public override void MainFunc()
    {
        if(enemy.IsDamaged)
        {
            machine.TransitionTo(damagedID);
            return;
        }

        // 1フレーム当たりの移動量
        float moveValue = moveSpeed * Time.deltaTime;
        if(machine.Cnt >= moveTime)
        {
            // 超過量をもどす
            float gap = machine.Cnt - moveTime;
            moveValue -= gap * moveSpeed;
        }


        if (nowIsUp)
        {
            enemy.transform.Translate(enemy.transform.up * moveValue);
        }
        else
        {
            enemy.transform.Translate(-enemy.transform.up * moveValue);
        }

        if(machine.Cnt >= moveTime)
        {
            nowIsUp = !nowIsUp;
            machine.TransitionTo(elapsedID);
        }

        if(enemy.IsFindPlayer)
        {
            machine.TransitionTo(searchSuccessID);
        }
    }
}
