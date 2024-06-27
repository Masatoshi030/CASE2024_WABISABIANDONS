using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_B_Attack : EnemyState
{
    [SerializeField, Header("�U���Ԋu")]
    float attackInterval = 3.0f;
    [SerializeField, Header("�U�����̐����I�u�W�F�N�g")]
    GameObject attackObject;
    [SerializeField, Header("�U���I�u�W�F�N�g�̑��x")]
    float attackSpeed;
    [SerializeField, Header("�U���I�u�W�F�N�g�̃p���[")]
    float attackPower;
    [SerializeField, Header("����")]
    float recoil = 1.5f;

    [Space(pad), Header("--�J�ڐ惊�X�g--")]
    [SerializeField, Header("�o�ߎ��̑J��")]
    StateKey elapsedKey;
    [SerializeField, Header("�������̑J��")]
    StateKey findingKey;
    [SerializeField, Header("�_���[�W���̑J��")]
    StateKey damagedKey;

    public override void Initialize()
    {
        base.Initialize();
    }

    public override void Enter()
    {
        base.Enter();

        // �e��΂�
        GameObject attackObj = Instantiate(attackObject, enemy.transform.position, Quaternion.identity);
        attackObj.transform.forward = enemy.EyeTransform.forward;
        attackObj.transform.LookAt(PlayerController.instance.transform.position);

        // �㉺��Velocity�������킹��
        Vector3 direction = (Enemy.Target.transform.position - attackObj.transform.position).normalized;
        direction *= attackSpeed;
        attackObj.GetComponent<Rigidbody>().velocity = direction;

        // ����
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
