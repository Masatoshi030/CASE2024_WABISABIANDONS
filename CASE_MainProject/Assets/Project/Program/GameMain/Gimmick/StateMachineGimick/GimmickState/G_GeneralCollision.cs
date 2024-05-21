using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G_GeneralCollision : GimmickState
{
    [Space(pad), Header("--�J�ڐ惊�X�g--")]
    [SerializeField, Header("�Փˎ��̑J��")]
    string colllisionTransition;

    public override void Enter()
    {
        base.Enter();
    }

    public override void CollisionEnterSelf(GameObject other)
    {
        if (other.transform.root.name == "Player")
        {
            if (PlayerController.instance.attackState == PlayerController.ATTACK_STATE.Attack)
            {
                machine.TransitionTo(colllisionTransition);
            }
        }
    }

    public override void TriggerEnterSelf(GameObject other)
    {
        if (other.transform.root.name == "Player")
        {
            if(PlayerController.instance.attackState == PlayerController.ATTACK_STATE.Attack)
            {
                machine.TransitionTo(colllisionTransition);
            }
        }
    }
}
