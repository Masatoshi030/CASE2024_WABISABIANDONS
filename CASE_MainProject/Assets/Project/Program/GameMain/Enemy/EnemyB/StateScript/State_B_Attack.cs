using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_B_Attack : EnemyState
{
    [SerializeField, Header("攻撃間隔")]
    float attackInterval = 3.0f;
    [SerializeField, Header("攻撃時の生成オブジェクト")]
    GameObject attackObject;
    [SerializeField, Header("攻撃オブジェクトの速度")]
    float attackSpeed;
    [SerializeField, Header("攻撃オブジェクトのパワー")]
    float attackPower;
    [SerializeField, Header("反動")]
    float recoil = 1.5f;

    [Space(pad), Header("--遷移先リスト--")]
    [SerializeField, Header("経過時の遷移")]
    StateKey elapsedKey;
    [SerializeField, Header("発見時の遷移")]
    StateKey findingKey;
    [SerializeField, Header("ダメージ時の遷移")]
    StateKey damagedKey;

    public override void Initialize()
    {
        base.Initialize();
    }

    public override void Enter()
    {
        base.Enter();

        // 弾飛ばす
        GameObject attackObj = Instantiate(attackObject, enemy.transform.position, Quaternion.identity);
        attackObj.transform.forward = enemy.EyeTransform.forward;
        attackObj.transform.LookAt(PlayerController.instance.transform.position);

        // 上下のVelocityだけ合わせる
        Vector3 direction = (Enemy.Target.transform.position - attackObj.transform.position).normalized;
        direction *= attackSpeed;
        attackObj.GetComponent<Rigidbody>().velocity = direction;

        // 反動
        enemy.EnemyRigidbody.velocity = (-enemy.transform.forward) * recoil;
    }

    public override void MainFunc()
    {
        base.MainFunc();

        if (enemy.IsDamaged)
        {
            machine.TransitionTo(damagedKey);
            return;
        }
        else if (machine.Cnt >= attackInterval)
        {
            if(enemy.IsFindPlayer)
            {
                machine.TransitionTo(findingKey);
            }
            else
            {
                machine.TransitionTo(elapsedKey);
            }
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
