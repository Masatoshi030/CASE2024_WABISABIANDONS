using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G_General_Running : GimmickState
{
    [SerializeField, Header("作動時間")]
    float runningTime;
    [Space(pad), Header("--遷移先リスト--")]
    [SerializeField, Header("時間経過後の遷移")]
    string elapsedTransition;

    public override void Enter()
    {
        base.Enter();
    }

    public override void MainFunc()
    {
        if (machine.Cnt >= runningTime) machine.TransitionTo(elapsedTransition);
    }

    public override void CollisionEnterSelf(Collision collision)
    {
        if (collision.transform.root.name == "Player")
        {
            if (PlayerController.instance.attackState == PlayerController.ATTACK_STATE.Attack)
            {
                Vector3 direction = collision.transform.position - gimmick.transform.position;
                direction.Normalize();
                direction.y = 0.9f;
                // 仮引数
                PlayerController.instance.KnockBack(10.0f, direction);
            }
        }
    }

    public override void TriggerEnterSelf(Collider other)
    {
        if (other.transform.root.name == "Player")
        {
            if (PlayerController.instance.attackState == PlayerController.ATTACK_STATE.Attack)
            {
                Vector3 direction = other.transform.position - gimmick.transform.position;
                direction.Normalize();
                direction.y = 0.9f;
                // 仮引数
                PlayerController.instance.KnockBack(10.0f, direction);
            }
        }
    }
}
