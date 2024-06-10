using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class General_IdleNoSearch : EnemyState
{
    [SerializeField, Header("待機時間")]
    float waitInterval;
    [Space(pad), Header("--遷移先リスト--")]
    [SerializeField, Header("時間経過後の遷移")]
    public int elapsedID;
    [SerializeField, Header("被弾時の遷移")]
    public int damagedID;

    public override void Enter()
    {
        base.Enter();

        enemy.IsSearchPlayer = false;
        enemy.IsVelocityZero = true;
    }

    public override void MainFunc()
    {
        base.MainFunc();

        if (enemy.IsDamaged)
        {
            enemy.IsDamaged = false;
            machine.TransitionTo(damagedID);
            return;
        }
        else if (machine.Cnt >= waitInterval)
        {
            machine.TransitionTo(elapsedID);
        }
    }

    public override void Exit()
    {
        base.Exit();

        enemy.IsSearchPlayer = true;
    }

    public override void CollisionEnterSelf(Collision collision)
    {
        if(collision.transform.name == "Player" && !enemy.IsDamaged)
        {
            Debug.Log("Collision");
            PlayerController.instance.Damage(5.0f);
        }
    }
}
