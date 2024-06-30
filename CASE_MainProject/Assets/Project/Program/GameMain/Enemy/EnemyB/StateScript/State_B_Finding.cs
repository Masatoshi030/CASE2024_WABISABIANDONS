using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_B_Finding : EnemyState
{
    [SerializeField, Header("��]���x")]
    float rotationSpeed;

    [SerializeField, Header("��]����")]
    float rotationTime;

    [SerializeField, Header("���̒���")]
    float allowLength = 10.0f;

    [SerializeField, Header("���ɓK�p����}�e���A��")]
    Material allowMaterial;

    [Space(pad), Header("--�J�ڐ惊�X�g--")]
    [SerializeField, Header("�o�ߎ��̑J��")]
    StateKey elapsedKey;
    [SerializeField, Header("�_���[�W���̑J��")]
    StateKey damagedKey;

    public override void Enter()
    {
        base.Enter();
        // ���̕\��
        enemy.SetAllowActive(true);
        enemy.AllowObject.GetComponent<TargetAllow>().SetMaterial(allowMaterial);

        // ���m�̒ʒm
        enemy.SendMsg<int>(0, 0);
        enemy.IsVelocityZero = true;
    }

    public override void MainFunc()
    {
        base.MainFunc();

        Quaternion verticalQuaternion = enemy.transform.rotation;
        verticalQuaternion.x = 0.0f;
        verticalQuaternion.z = 0.0f;
        Vector3 direction = (Enemy.Target.transform.position - enemy.EyeTransform.position).normalized;
        float Distance = Vector3.Distance(Enemy.Target.transform.position, enemy.EyeTransform.position);
        if(Distance < allowLength)
        {
            enemy.AllowObject.GetComponent<TargetAllow>().StartDesignation(enemy.EyeTransform.position, enemy.EyeTransform.position + direction * Distance);
        }
        else
        {
            enemy.AllowObject.GetComponent<TargetAllow>().StartDesignation(enemy.EyeTransform.position, enemy.EyeTransform.position + direction * allowLength);
        }
        direction = (Enemy.Target.transform.position - enemy.transform.position).normalized;
        Quaternion rotation = Quaternion.LookRotation(direction, enemy.transform.up);
        rotation *= Quaternion.Euler(0, rotationSpeed * Time.deltaTime, 0.0f);
        rotation.x = 0.0f;
        rotation.z = 0.0f;

        enemy.transform.rotation = Quaternion.Lerp(enemy.transform.rotation, rotation, Time.deltaTime * 10);

        if (enemy.IsDamaged)
        {
            machine.TransitionTo(damagedKey);
        }
        else if (machine.Cnt >= rotationTime)
        {
            machine.TransitionTo(elapsedKey);
        }
    }

    public override void Exit()
    {
        base.Exit();
        enemy.SetAllowActive(false);
        enemy.AllowObject.GetComponent<TargetAllow>().EndDesignation();
        enemy.IsVelocityZero = false;
    }
}
