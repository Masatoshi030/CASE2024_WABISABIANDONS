using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyB_Attack : EnemyState
{
    [SerializeField, Header("�U���Ԋu")]
    float attackDuration = 3.0f;
    [SerializeField, Header("�U�����̐����I�u�W�F�N�g")]
    GameObject attackObject;
    [SerializeField, Header("�U���I�u�W�F�N�g�̑��x")]
    float attackPower;

    [Space(pad), Header("--�J�ڐ惊�X�g--")]
    [SerializeField, Header("�ǐՎ��s���̑J��")]
    public string failedTransition = "�ҋ@";
    [SerializeField, Header("�������ȓ��̑J��")]
    public string successfulTransition = "�U��";
    [SerializeField, Header("��e���̑J��")]
    public string damagedTransition = "��e";
    [SerializeField, Header("�Փˎ��̑J��")]
    public string collisionTransition = "�ҋ@";

    public override void Initialize()
    {
        
    }

    public override void Enter()
    {
        base.Enter();
        enemy.EnemyRigidbody.velocity = Vector3.zero;
    }

    public override void MainFunc()
    {
        
    }

    public override void Exit()
    {

    }

    public override void CollisionEnterOpponent(GameObject other)
    {
        if (other.transform.root.name == "Player")
        {
            if (PlayerController.instance.attackState == PlayerController.ATTACK_STATE.Attack)
                machine.TransitionTo(damagedTransition);
            else
                machine.TransitionTo(collisionTransition);
        }
    }

    public override void TriggerEnterOpponent(GameObject other)
    {
        if (other.transform.root.name == "Player")
        {
            if (PlayerController.instance.attackState == PlayerController.ATTACK_STATE.Attack)
                machine.TransitionTo(damagedTransition);
            else
                machine.TransitionTo(collisionTransition);
        }
    }
}
