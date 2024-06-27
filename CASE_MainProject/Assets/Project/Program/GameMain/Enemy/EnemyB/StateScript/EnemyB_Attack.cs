using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyB_Attack : EnemyState
{
    [SerializeField, Header("�U���Ԋu")]
    float attackInterval = 3.0f;
    [SerializeField, Header("�U�����̐����I�u�W�F�N�g")]
    GameObject attackObject;
    [SerializeField, Header("�U���I�u�W�F�N�g�̑��x")]
    float attackSpeed;
    [SerializeField, Header("�U���I�u�W�F�N�g�̃p���[")]
    float attackPower;
    [SerializeField, Header("��]���x")]
    float rotationSpeed = 0.8f;

    [Space(pad), Header("--�J�ڐ惊�X�g--")]
    [SerializeField, Header("������̑J��ID")]
    public int elapsedID;
    [SerializeField, Header("��e���̑J��")]
    public int damagedID;
    [SerializeField, Header("�Փˎ��̑J��")]
    public int collisionID;

    public override void Initialize()
    {
        base.Initialize();
    }

    public override void Enter()
    {
        base.Enter();

        enemy.EnemyRigidbody.velocity = Vector3.zero;
    }

    public override void MainFunc()
    {
        base.MainFunc();

        float angleY = enemy.transform.rotation.y;
        float angle = Mathf.Lerp(0.0f, enemy.ToPlayerAngle, rotationSpeed);

        enemy.transform.Rotate(0.0f, angle * Time.deltaTime, 0.0f);

        if (enemy.IsDamaged)
        {
            machine.TransitionTo(damagedID);
            return;
        }
        else if (machine.Cnt >= attackInterval)
        {
            GameObject attackObj = Instantiate(attackObject, enemy.transform.position, Quaternion.identity);
            attackObj.transform.forward = enemy.EyeTransform.forward;
            if(Mathf.Abs(enemy.ToPlayerAngle) < 20.0f)
            {
                attackObj.transform.LookAt(PlayerController.instance.transform.position);
            }
            // �㉺��Velocity�������킹��
            float verticalVelocity = PlayerController.instance.transform.position.y - attackObj.transform.position.y;
            attackObj.GetComponent<DrillComponent>().AttackPower = attackPower;
            Vector3 velocity = attackObj.transform.forward * attackSpeed;
            velocity.y = verticalVelocity;
            attackObj.GetComponent<Rigidbody>().velocity = velocity;
            machine.TransitionTo(elapsedID);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void CollisionEnterOpponent(Collision collision)
    {
        if (collision.transform.root.name == "Player")
        {
            if (PlayerController.instance.attackState != PlayerController.ATTACK_STATE.Attack)
                machine.TransitionTo(collisionID);
        }
    }

    public override void TriggerEnterOpponent(Collider other)
    {
        if (other.transform.root.name == "Player")
        {
            if (PlayerController.instance.attackState != PlayerController.ATTACK_STATE.Attack)
                machine.TransitionTo(collisionID);
        }
    }
}
