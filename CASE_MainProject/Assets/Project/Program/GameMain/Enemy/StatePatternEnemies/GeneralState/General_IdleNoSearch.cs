using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class General_IdleNoSearch : EnemyState
{
    [SerializeField, Header("�ҋ@����")]
    float waitInterval;
    [SerializeField, Header("�o�ߌ�̑J��")]
    public string elapsedTransition = "�ړ�";
    [SerializeField, Header("��e���̑J��")]
    public string damagedTransition = "��e";

    public override void Enter()
    {
        base.Enter();
        enemy.IsSearchPlayer = false;
    }

    public override void MainFunc()
    {
        if (machine.Cnt >= waitInterval) machine.TransitionTo(elapsedTransition);
    }

    public override void Exit()
    {
        enemy.IsSearchPlayer = true;
    }

    public override void CollisionEnter(Collision collision)
    {
        if (collision.transform.root.name == "Player")
        {
            if (PlayerController.instance.attackState == PlayerController.ATTACK_STATE.Attack) machine.TransitionTo(damagedTransition);
        }
    }

    public override void TriggerEnter(Collider collider)
    {
        if (collider.transform.root.name == "Player")
        {
            if (PlayerController.instance.attackState == PlayerController.ATTACK_STATE.Attack) machine.TransitionTo(damagedTransition);
        }
    }
}
