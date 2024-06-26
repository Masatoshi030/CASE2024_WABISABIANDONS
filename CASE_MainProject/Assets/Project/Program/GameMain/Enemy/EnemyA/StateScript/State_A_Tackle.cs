using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_A_Tackle : EnemyState
{
    [SerializeField, Header("�^�b�N���̋���(velocity)")]
    float tackleVelocity = 10.0f;
    [SerializeField, Header("�^�b�N���̋���(damage)")]
    float tacklePower = 10.0f;
    [SerializeField, Header("�������v���C���[�ɍ��킹�邩")]
    bool isCalcPlayerHeight = false;
    [SerializeField, Header("�^�b�N���̏�����̋���")]
    float tackleUpVelocity = 1.0f;
    [SerializeField, Header("�^�b�N����̍d������")]
    float stiffnessTime = 3.0f;
    [SerializeField, Header("�^�b�N������"), ReadOnly]
    bool bTackle = false;
    float subCnt = 0.0f;

    [Space(pad), Header("--�J�ڐ惊�X�g--")]
    [SerializeField, Header("�Փˎ��̑J��")]
    StateKey tackledKey;
    [SerializeField, Header("�_���[�W���̑J��")]
    StateKey damagedKey;

    public override void Enter()
    {
        base.Enter();
        enemy.IsVelocityZero = false;
        // y���ȊO�̐���������
        Quaternion yOnly = enemy.transform.rotation;
        yOnly.x = 0.0f;
        yOnly.z = 0.0f;
        enemy.transform.rotation = yOnly;
        // �^�b�N������
        Vector3 velocity;
        if (isCalcPlayerHeight)
        {
            velocity = (Enemy.Target.transform.position - enemy.transform.position).normalized;
            velocity *= tackleVelocity;
            velocity.y += 3.0f;
        }
        else
        {
            velocity = enemy.transform.forward;
            velocity.x *= tackleVelocity;
            velocity.z *= tackleVelocity;
            velocity.y = tackleUpVelocity;
            
        }
        enemy.EnemyRigidbody.velocity = velocity;
        enemy.EnemyCollider.isTrigger = true;
        bTackle = true;
        subCnt = 0.0f;
    }

    public override void MainFunc()
    {
        base.MainFunc();

        if (!bTackle) subCnt += Time.deltaTime;
        if(!bTackle && enemy.IsDamaged)
        {
            machine.TransitionTo(damagedKey);
        }
        else if(bTackle)
        {
            if (enemy.EnemyRigidbody.velocity.magnitude <= 1.0f)
            {
                bTackle = false;
                enemy.IsVelocityZero = true;
            }
        }

        if(!bTackle)
        {
            Ray ray = new Ray(enemy.EyeTransform.position, Vector3.down);
            RaycastHit[] hits = Physics.RaycastAll(ray, 3.0f);
            foreach (RaycastHit hit in hits)
            {
                if (hit.collider.transform.tag == "Ground")
                {
                    machine.TransitionTo(tackledKey);
                }
            }
        }
    }

    public override void Exit()
    {
        base.Exit();
        bTackle = false;
    }

    public override void CollisionStaySelf(Collision collision)
    {
        base.CollisionEnterSelf(collision);

        if (collision.transform.tag == "Ground")
        {
            Ray ray = new Ray(enemy.transform.position, Vector3.down);
            RaycastHit[] hits = Physics.RaycastAll(ray, 2.0f);
            foreach (RaycastHit hit in hits)
            {
                if(hit.collider.gameObject == collision.gameObject)
                {
                    machine.TransitionTo(tackledKey);
                }
            }
        }
    }

    public override void TriggerEnterSelf(Collider other)
    {
        base.TriggerEnterSelf(other);
        if(bTackle)
        {
            if(other.transform.tag == "Player")
            {
                if (PlayerController.instance.attackState == PlayerController.ATTACK_STATE.Attack)
                {
                    enemy.EnemyCollider.isTrigger = false;
                    bTackle = false;
                }
                else
                {
                    enemy.EnemyCollider.isTrigger = false;
                    PlayerController.instance.Damage(tacklePower);
                    bTackle = false;
                }
            }
            if (other.transform.tag == "Wall" || other.transform.tag == "Ground")
            {
                bTackle = false;
                enemy.EnemyRigidbody.velocity = Vector3.zero;
                enemy.EnemyCollider.isTrigger = false;
            }
        }
        else
        {
            if (other.transform.tag == "Wall" || other.transform.tag == "Ground")
            {
                enemy.EnemyCollider.isTrigger = false;
                enemy.EnemyRigidbody.velocity = Vector3.zero;
            }
        }
    }
}
