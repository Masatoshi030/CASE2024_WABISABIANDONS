using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyC_Rolling : EnemyState_C
{
    Rigidbody rb;

    [SerializeField, Header("���x")]
    float rollSpeed;
    [SerializeField, Header("����")]
    Vector3 direction;
    [SerializeField, Header("��]����{�f�B")]
    GameObject rollingBody;
    [SerializeField, Header("��]��"), VectorRange(-1.0f, 1.0f), ReadOnly]
    Vector3 rollAxis;
    float angle;

    [SerializeField, Header("�������݃I�u�W�F�N�g")]
    List<GameObject> implicateObjects;
    Dictionary<GameObject, Vector3> positions = new Dictionary<GameObject, Vector3>();
    Dictionary<GameObject, float> angles = new Dictionary<GameObject, float>();

    [Space(pad), Header("--�J�ڐ惊�X�g--")]
    [SerializeField, Header("�Փˎ��̑J��")]
    string collisionTransition = "���S";

    public override void Initialize()
    {
        base.Initialize();
        rb = enemy.transform.GetComponent<Rigidbody>();
        rollingBody = enemy.transform.Find("Body").gameObject;
    }

    public override void Enter()
    {
        rollSpeed = enemyC.DamageValue;
        direction = enemyC.DamageVector;
        direction.Normalize();

        Vector3 u = (Mathf.Abs(direction.z) < 0.0001f) ? Vector3.forward : Vector3.up;
        rollAxis = Vector3.Cross(direction, u);
        rollAxis.Normalize();
    }

    public override void MainFunc()
    {
        angle -= rollSpeed;
        angle %= -360.0f;
        enemyC.gameObject.transform.Translate(direction * rollSpeed * Time.deltaTime);
        rollingBody.transform.Rotate(rollAxis, -rollSpeed);

        for(int i = 0; i <  implicateObjects.Count; i++)
        {�@�@
            implicateObjects[i].transform.position = enemyC.transform.position + positions[implicateObjects[i]];
            implicateObjects[i].transform.RotateAround(enemyC.transform.position, rollAxis, angle + angles[implicateObjects[i]]);
        }
    }

    public override void CollisionEnterSelf(Collision collision)
    {
        if(collision.transform.tag == "Ground" || collision.transform.tag == "Wall")
        {
            // ���˃x�N�g���̌v�Z
            Vector3 point = collision.contacts[0].point;
            Vector3 localdirection = point - enemy.transform.position;

            Ray ray = new Ray(enemy.transform.position, localdirection);
            Vector3 normal = Vector3.zero;
            RaycastHit[] hits = Physics.RaycastAll(ray, 10.0f);
            for (int i = 0; i < hits.Length; i++)
            {
                if(hits[i].transform.gameObject == collision.gameObject)
                {
                    normal = hits[i].normal;
                }
            }
            if(normal == Vector3.zero)
            {
                normal = collision.contacts[0].normal;
            }

            Vector3 reflect = direction - 2 * normal * Vector3.Dot(direction, normal);
            reflect.y = 0.0f;
            reflect.Normalize();
            // �Փˎ��̈ړ������ƏՓ˃I�u�W�F�N�g�̖@���̌����̓��ς�ۑ����Ă���
            float dot = Vector3.Dot(direction, normal);
            if (dot < 0)
            {
                // ���͂̏�Ԃŕω�
                switch (enemyC.CState)
                {
                    case EnemyC.PressureState.Empty: enemyC.CState = EnemyC.PressureState.Medium; break;
                    case EnemyC.PressureState.Medium: enemyC.CState = EnemyC.PressureState.Full; break;
                    case EnemyC.PressureState.Full: 
                        // �ő�̏ꍇ
                        for(int i = 0; i < implicateObjects.Count;i++)
                        {
                            if (implicateObjects[i].GetComponent<Enemy>() != null)
                            {
                                Vector3 velocity = implicateObjects[i].transform.position - enemy.transform.position;
                                velocity.Normalize();
                                velocity.y = 5.0f;
                                Enemy enem = implicateObjects[i].GetComponent<Enemy>();
                                enem.GetComponent<NavMeshAgent>().enabled = false;
                                enem.EnemyRigidbody.AddForce(velocity * rollSpeed * 5.0f, ForceMode.Impulse);
                                enem.Machine.TransitionTo("���S�ҋ@");
                            }
                            positions.Remove(implicateObjects[i]);
                            angles.Remove(implicateObjects[i]);
                            implicateObjects.RemoveAt(i);
                        }
                        machine.TransitionTo(collisionTransition);
                        break;
                }
            }
            // �ړ������𔽎˃x�N�g���ɕϊ�
            direction = reflect;
            Vector3 u = (Mathf.Abs(direction.z) < 0.0001f) ? Vector3.forward : Vector3.up;
            rollAxis = Vector3.Cross(direction, u);
            rollAxis.Normalize();
            angle = 0.0f;
        }

        // �G�ɂԂ������ꍇ��������
        if(collision.transform.tag == "Enemy")
        {
            if (!implicateObjects.Contains(collision.gameObject))
            {
                // �Փ˃I�u�W�F�N�g���X�g�ɒǉ�
                implicateObjects.Add(collision.gameObject);
                // �Փˎ��̃x�N�g���̍��������߂�
                Vector3 sub = collision.transform.position - enemyC.transform.position;
                // �����������ɓo�^
                positions.Add(collision.gameObject, sub);
                // �Փˎ��̉�]�p�x��ۑ����Ă���
                angles.Add(collision.gameObject, angle);

                Enemy enem = collision.gameObject.GetComponent<Enemy>();
                enem.Machine.StateObject.AddComponent<EnemyC_Caught>();
                EnemyC_Caught state = enem.Machine.StateObject.GetComponent<EnemyC_Caught>();
                state.StateName = "��������";
                collision.gameObject.GetComponent<Enemy>().Machine.AddState(state);
                state.Caught = true;
                collision.gameObject.GetComponent<EnemyStateMachine>().TransitionTo("��������");

                enem.Machine.StateObject.AddComponent<EnemyC_IntervalDeath>();
                EnemyC_IntervalDeath deathState = enem.Machine.StateObject.GetComponent<EnemyC_IntervalDeath>();
                deathState.waitInterval = 2.0f;
                deathState.StateName = "���S�ҋ@";
                collision.gameObject.GetComponent<EnemyStateMachine>().AddState(deathState);
            }
        }
    }
}
