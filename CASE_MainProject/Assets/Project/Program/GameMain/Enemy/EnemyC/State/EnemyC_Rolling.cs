using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyC_Rolling : EnemyState_C
{
    Rigidbody rb;

    [SerializeField, Header("���x"), ReadOnly]
    float rollSpeed;
    [SerializeField, Header("����"), ReadOnly]
    Vector3 direction;
    [SerializeField, Header("��]����{�f�B")]
    GameObject rollingBody;
    [SerializeField, Header("��]��"), ReadOnly, VectorRange(-1.0f, 1.0f)]
    Vector3 rollAxis;
    float angle;

    [SerializeField, Header("�o���u�{�[�i�X(value * �������ݐ�)")]
    uint valveBonus = 5;

    [SerializeField, Header("�������݃I�u�W�F�N�g")]
    List<GameObject> implicateObjects;
    Dictionary<GameObject, Vector3> positions = new Dictionary<GameObject, Vector3>();
    Dictionary<GameObject, float> angles = new Dictionary<GameObject, float>();

    [Space(pad), Header("--�J�ڐ惊�X�g--")]
    [SerializeField, Header("�Փˎ��̑J��")]
    int collisionID;

    public override void Initialize()
    {
        base.Initialize();

        rb = enemy.transform.GetComponent<Rigidbody>();
        rollingBody = enemy.transform.Find("Body").gameObject;
    }

    public override void Enter()
    {
        base.Enter();
        enemy.IsVelocityZero = false;
        enemy.gameObject.GetComponent<NavMeshAgent>().enabled = false;
        rollSpeed = enemyC.DamageValue;
        direction = enemy.DamageVector;
        direction.y = 0.0f;
        direction.Normalize();
        enemy.transform.LookAt(enemy.transform.position + direction * 20.0f);
    }

    public override void MainFunc()
    {
        base.MainFunc();

        angle -= rollSpeed;
        angle %= -360.0f;
        //enemy.transform.Translate(enemy.transform.forward *  rollSpeed * Time.deltaTime);

        enemy.transform.position += direction * rollSpeed * Time.deltaTime;

        Vector3 rotateValue = new Vector3(0.0f, 0.0f, -rollSpeed);
        rollingBody.transform.Rotate(rotateValue);

        for (int i = 0; i < implicateObjects.Count; i++)
        {
            implicateObjects[i].transform.position = enemyC.transform.position + positions[implicateObjects[i]];
            implicateObjects[i].transform.RotateAround(enemyC.transform.position, rollAxis, angle + angles[implicateObjects[i]]);
        }
    }

    public override void CollisionEnterSelf(Collision collision)
    {
        if (collision.transform.tag == "Ground")
        {
            Ray ray = new Ray(enemy.EyeTransform.position, enemy.EyeTransform.forward);
            RaycastHit[] hits = Physics.RaycastAll(ray, 1.0f);
            foreach (RaycastHit hit in hits)
            {
                if (hit.collider.gameObject == collision.gameObject)
                {
                    // ���˃x�N�g���̌v�Z
                    enemy.EnemyRigidbody.velocity = Vector3.zero;
                    Vector3 normal = collision.contacts[0].normal;

                    Vector3 reflect = direction - 2 * normal * Vector3.Dot(direction, normal);
                    reflect.y = 0.0f;
                    reflect.Normalize();

                    // �ړ������𔽎˃x�N�g���ɕϊ�
                    direction = reflect;
                    enemy.transform.LookAt(enemy.transform.position + direction);

                    float dot = Vector3.Dot(direction, normal);

                    switch (enemyC.CState)
                    {
                        case EnemyC.PressureState.Low:
                            enemyC.CState = EnemyC.PressureState.Med;
                            enemyC.applyMesh.material = EnemyC_Manager.instance.Material1;
                            EnemyC_Manager.instance.CreateCollisionEffect(enemy.transform.position, Quaternion.identity);
                            break;
                        case EnemyC.PressureState.Med:
                            enemyC.CState = EnemyC.PressureState.High;
                            enemyC.applyMesh.material = EnemyC_Manager.instance.Material0;
                            EnemyC_Manager.instance.CreateCollisionEffect(enemy.transform.position, Quaternion.identity);
                            break;
                        case EnemyC.PressureState.High:
                            // �ő�̏ꍇ
                            ExplosionMyBody();
                            break;
                    }
                }
            }
        }
        else if (collision.transform.tag == "Wall")
        {
            // ���˃x�N�g���̌v�Z
            enemy.EnemyRigidbody.velocity = Vector3.zero;
            Vector3 normal = collision.contacts[0].normal;

            Vector3 reflect = direction - 2 * normal * Vector3.Dot(direction, normal);
            reflect.y = 0.0f;
            reflect.Normalize();

            // �ړ������𔽎˃x�N�g���ɕϊ�
            direction = reflect;
            enemy.transform.LookAt(enemy.transform.position + direction);

            switch (enemyC.CState)
            {
                case EnemyC.PressureState.Low:
                    enemyC.CState = EnemyC.PressureState.Med;
                    enemyC.applyMesh.material = EnemyC_Manager.instance.Material1;
                    EnemyC_Manager.instance.CreateCollisionEffect(enemy.transform.position, Quaternion.identity);
                    break;
                case EnemyC.PressureState.Med:
                    enemyC.CState = EnemyC.PressureState.High;
                    enemyC.applyMesh.material = EnemyC_Manager.instance.Material0;
                    EnemyC_Manager.instance.CreateCollisionEffect(enemy.transform.position, Quaternion.identity);
                    break;
                case EnemyC.PressureState.High:
                    // �ő�̏ꍇ
                    ExplosionMyBody();
                    break;
            }
        }
        // �G�ɂԂ������ꍇ��������
        else if (collision.transform.tag == "Enemy")
        {
            EnemyC_Manager.instance.CreateCollisionEffect(enemy.transform.position, Quaternion.identity);
            enemy.EnemyRigidbody.velocity = Vector3.zero;
            // �I�u�W�F�N�g�����o�^
            if (!implicateObjects.Contains(collision.gameObject))
            {
                Enemy enem = collision.gameObject.GetComponent<Enemy>();
                // �X�e�[�g�̎擾������
                if (enem.Machine.StateObject.GetComponent<EnemyC_Caught>())
                {
                    // �ߑ���Ԃ̃X�e�[�g���擾
                    EnemyC_Caught caughtState = enem.Machine.StateObject.GetComponent<EnemyC_Caught>();
                    caughtState.Caught = true;

                    // �o���u�𗎂Ƃ��Ȃ���Ԃɂ���
                    enem.IsDropValves = false;

                    // �擾�����X�e�[�g��ID�����ɑJ��
                    enem.Machine.TransitionTo(caughtState.StateID);

                    // �R���C�_�[�̔�A�N�e�B�u��
                    enem.EnemyCollider.enabled = false;
                    if (enem.EnemyAgent != null)
                        enem.EnemyAgent.enabled = false;

                    // �Փ˃I�u�W�F�N�g���X�g�ɒǉ�
                    implicateObjects.Add(collision.gameObject);

                    // �Փˎ��̃x�N�g���̍��������߂�
                    Vector3 sub = collision.transform.position - enemyC.transform.position;

                    // �����������ɓo�^
                    positions.Add(collision.gameObject, sub);

                    // �Փˎ��̉�]�p�x��ۑ����Ă���
                    angles.Add(collision.gameObject, angle);
                    if(enem.GetComponent<EnemyC>() == null)
                    {
                        enem.UnSubscribeAll();
                    }
                }
            }
        }
        else if (collision.transform.tag == "GoldValve")
        {
            // �擾��ԂɕύX
            collision.transform.GetComponent<GoldValveController>().GetGoldValve();
        }
        else if (collision.transform.tag == "Valve")
        {
            // �o���u�M�~�b�N�̃R���|�[���Ǝ擾
            if (collision.transform.GetComponent<Valve_Base>())
            {
                // �M�~�b�N�̍쓮
                collision.transform.GetComponent<Valve_Base>().SetGimmickCommand();

                ExplosionMyBody();
            }
        }
    }

    public override void TriggerEnterSelf(Collider other)
    {
        if (other.transform.tag == "Enemy")
        {
            EnemyC_Manager.instance.CreateCollisionEffect(enemy.transform.position, Quaternion.identity);
            enemy.EnemyRigidbody.velocity = Vector3.zero;
            // �I�u�W�F�N�g�����o�^
            if (!implicateObjects.Contains(other.gameObject))
            {
                Enemy enem = other.gameObject.GetComponent<Enemy>();
                // �X�e�[�g�̎擾������
                if (enem.Machine.StateObject.GetComponent<EnemyC_Caught>())
                {
                    EnemyC_Caught caughtState = enem.Machine.StateObject.GetComponent<EnemyC_Caught>();
                    caughtState.Caught = true;

                    // �o���u��h���b�v���
                    enem.IsDropValves = false;

                    enem.Machine.TransitionTo(caughtState.StateID);
                    enem.EnemyCollider.enabled = false;
                    // �Փ˃I�u�W�F�N�g���X�g�ɒǉ�
                    implicateObjects.Add(other.gameObject);
                    // �Փˎ��̃x�N�g���̍��������߂�
                    Vector3 sub = other.transform.position - enemyC.transform.position;
                    // �����������ɓo�^
                    positions.Add(other.gameObject, sub);
                    // �Փˎ��̉�]�p�x��ۑ����Ă���
                    angles.Add(other.gameObject, angle);
                    if (enem.GetComponent<EnemyC>() == null)
                    {
                        enem.UnSubscribeAll();
                    }
                }
            }
        }
        else if (other.transform.tag == "GoldValve")
        {
            other.transform.GetComponent<GoldValveController>().GetGoldValve();
        }
        else if (other.transform.tag == "Valve")
        {
            if (other.transform.GetComponent<Valve_Base>())
            {
                other.transform.GetComponent<Valve_Base>().SetGimmickCommand();
                ExplosionMyBody();
            }
        }
        else if(other.transform.tag == "Player")
        {
            if(PlayerController.instance.attackState != PlayerController.ATTACK_STATE.Attack)
            {
                PlayerController.instance.Damage(20.0f + 5.0f * (int)enemyC.CState);
            }
            else
            {

            }
        }
    }

    void ExplosionMyBody()
    {
        // �o���u�̃X�|�[����
        uint spawnNum = 0;
        uint Count = 0;

        foreach (GameObject obj in implicateObjects)
        {
            if (obj.GetComponent<Enemy>() != null)
            {
                Vector3 velocity = obj.transform.position - enemy.transform.position;
                velocity.Normalize();
                velocity.y = 5.0f;
                Enemy enem = obj.GetComponent<Enemy>();

                // �h���b�v����o���u�̎擾
                spawnNum += enem.DropValveNum;
                if (enem.DropValveNum > 0)
                {
                    Count++;
                }

                if (enem.GetComponent<NavMeshAgent>())
                    enem.GetComponent<NavMeshAgent>().enabled = false;
                enem.EnemyRigidbody.AddForce(velocity * rollSpeed * 2.0f, ForceMode.Impulse);
                if (enem.Machine.StateObject.GetComponent<EnemyC_IntervalDeath>())
                {
                    EnemyC_IntervalDeath deathState = enem.Machine.StateObject.GetComponent<EnemyC_IntervalDeath>();
                    enem.Machine.TransitionTo(deathState.StateID);
                }
            }
        }
        // �X�|�[��������o���u�Ƀ{�[�i�X�����Z
        if(Count > 4)
        {
            // ��{�̃{�[�i�X�o���u+2��
            spawnNum = spawnNum + ((valveBonus + 2) * Count);
        }
        else if(Count > 2)
        {
            spawnNum = spawnNum + ((valveBonus + 1) * Count);
        }
        else
        {
            spawnNum = spawnNum + (valveBonus * Count);
        }
        Debug.Log(spawnNum);
        DropValveManager.instance.CreateValves(spawnNum, enemy.transform.position, enemy.IsAutoGet);

        positions.Clear();
        angles.Clear();
        implicateObjects.Clear();
        EnemyC_Manager.instance.CreateExplosionEffect(enemy.transform.position, Quaternion.identity);
        machine.TransitionTo(collisionID);
    }

}

