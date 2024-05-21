using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G_GeneralCollision : GimmickState
{
    [Space(pad), Header("--遷移先リスト--")]
    [SerializeField, Header("衝突時の遷移")]
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
