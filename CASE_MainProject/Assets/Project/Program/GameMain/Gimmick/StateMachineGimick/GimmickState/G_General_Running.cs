using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G_General_Running : GimmickState
{
    [SerializeField, Header("�쓮����")]
    float runningTime;
    [Space(pad), Header("--�J�ڐ惊�X�g--")]
    [SerializeField, Header("���Ԍo�ߌ�̑J��")]
    string elapsedTransition;

    public override void Enter()
    {
        base.Enter();
    }

    public override void MainFunc()
    {
        if (machine.Cnt >= runningTime) machine.TransitionTo(elapsedTransition);
    }

    public override void CollisionEnterSelf(GameObject other)
    {
        if (other.transform.root.name == "Player")
        {
            if (PlayerController.instance.attackState == PlayerController.ATTACK_STATE.Attack)
            {
                Vector3 direction = other.transform.position - gimmick.transform.position;
                direction.Normalize();
                direction.y = 0.9f;
                // ������
                PlayerController.instance.KnockBack(10.0f, direction);
            }
        }
    }

    public override void TriggerEnterSelf(GameObject other)
    {
        if (other.transform.root.name == "Player")
        {
            if (PlayerController.instance.attackState == PlayerController.ATTACK_STATE.Attack)
            {
                Vector3 direction = other.transform.position - gimmick.transform.position;
                direction.Normalize();
                direction.y = 0.9f;
                // ������
                PlayerController.instance.KnockBack(10.0f, direction);
            }
        }
    }
}
