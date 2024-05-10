using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy_TypeA : Enemy_Mob
{
    [Space(padA), Header("--���C���p�����[�^--")]
    [SerializeField, Header("�ėp�J�E���g"), ReadOnly]
    float cnt = 0.0f;
    [Space(padB), Header("-�ړ�-")]
    [SerializeField, Header("�ő�ړ����x")]
    float moveSpeed = 5.0f;
    [SerializeField, Header("�����x")]
    float moveAcceleration = 2.0f;
    [Space(padB), Header("-�ǔ�-")]
    [SerializeField, Header("�ǔ����̑��x")]
    float trackingSpeed = 8.0f;
    [SerializeField, Header("�ǔ��������x")]
    float trackingAcceleration = 2.0f;
    [SerializeField, Header("�ێ����鋗��")]
    float trackingKeepDistance = 3.0f;
    [Space(padB), Header("-���S-")]
    [SerializeField, Header("���S���̑��x")]
    float escapeSpeed = 8.0f;
    [SerializeField, Header("���S�������x")]
    float escapeAcceleration = 2.0f;
    [SerializeField, Header("���S����")]
    float escapeTime = 1.2f;
    [SerializeField, Header("���S����")]
    float escapeDistance = 3.0f;
    

    [Space(padA), Header("--�C���^�[�o��--")]
    [SerializeField, Header("�ҋ@�C���^�[�o��")]
    float waitInterval = 3.0f;

    [Space(padA), Header("--�p�g���[���֘A--")]
    [SerializeField, Header("�R���|�[�l���g")]
    NavMeshPatrol patrol;
    [SerializeField, Header("�ړI�n��"), ReadOnly]
    int targetNum;
    [SerializeField, Header("���̖ړI�n�̓Y����"), ReadOnly]
    int targetIndex = 0;

    [SerializeField, Header("�A�j���[�^�[")]
    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        patrol = GetComponent<NavMeshPatrol>();
        animator = GetComponent<Animator>();
        targetNum = patrol.GetTargets().Length;
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
    }

    protected override void IdleFunc()
    {
        cnt += Time.deltaTime;
        if (FindPlayerAtFOV().isFind)
        {
            cnt = 0.0f;
            patrol.SetAgentParam(trackingSpeed, trackingAcceleration, true);
            state = State.Tracking;
            return;
        }

        if (cnt > waitInterval)
        {
            cnt = 0.0f;
            patrol.SetAgentParam(moveSpeed, moveAcceleration);
            patrol.ExcutePatrol(targetIndex);
            Debug.Log("�ړ�" + patrol.GetRemaingDistance());
            state = State.Move;
        }
    }

    protected override void MoveFunc()
    {
        if (FindPlayerAtFOV().isFind)
        {
            state = State.Tracking;
            patrol.SetAgentParam(trackingSpeed, trackingAcceleration, true);
            return;
        }

        if (patrol.GetPatrolState() == NavMeshPatrol.PatrolState.Idle)
        {
            targetIndex++;
            if(targetIndex >= targetNum)
            {
                targetIndex = 0;
            }
            state = State.Idle;
        }
    }

    protected override void TrackingFunc()
    {
        (bool isFind, float distance) = FindPlayerAtFOV();
        if (!isFind)
        {
            state = State.Idle;
            return;
        }

        if(Vector3.Dot(eyeTransform.forward, PlayerController.instance.transform.Find("HorizontalRotationShaft").Find("MoveShaft").transform.forward) < -0.75f)
        {
            state = State.Escape;
            patrol.SetAgentParam(escapeSpeed, escapeAcceleration);
            return;
        }

        // �ړI�n�̎Z�o
        Vector3 targetVector = transform.position - target.transform.position;
        targetVector.Normalize();
        Vector3 Position = target.transform.position + targetVector * trackingKeepDistance;
        patrol.ExcuteCustom(Position);
        Debug.Log("�ǔ�" + patrol.GetRemaingDistance());
    }

    protected override void EscapeFunc()
    {
        cnt += Time.deltaTime;
        if(cnt > escapeTime)
        {
            cnt = 0.0f;
            state = State.Idle;
            return;
        }
        // �ړI�n�̎Z�o
        Vector3 targetVector = transform.position - target.transform.position;
        targetVector.Normalize();
        Vector3 Position = transform.position + targetVector * escapeDistance;
        patrol.ExcuteCustom(Position);
    }

    protected override void DeathFunc()
    {
        Destroy(gameObject);
    }

    protected override void DestroyFunc()
    {
        animator.SetBool("bDeath", true);
    }
}
