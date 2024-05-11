using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy_TypeA : Enemy_Mob
{
    [Space(padA), Header("--���C���p�����[�^--")]
    [SerializeField, Header("�ėp�J�E���g"), ReadOnly]
    float cnt = 0.0f;
    [Space(padB), Header("-�ҋ@�p�����[�^-")]
    [SerializeField, Header("�ҋ@�C���^�[�o��")]
    float waitInterval = 3.0f;
    [Space(padB),SerializeField, Header("-�ړ��p�����[�^-")]
    EnemyAgentParam moveParam;
    [Space(padB), SerializeField, Header("-�ǔ��p�����[�^-")]
    EnemyAgentParam trackingParam;
    [SerializeField, Header("�ǔ����Ɉێ����鋗��")]
    float trackingKeepDistance;
    [Space(padB), SerializeField, Header("-���S�p�����[�^-")]
    EnemyAgentParam escapeParam;
    [SerializeField, Header("���S����")]
    float escapeTime;
    [SerializeField, Header("���S����")]
    float escapeDistance;
    [Space(padB), SerializeField, Header("-�񕜃p�����[�^-")]
    HealParam healParam;

    

    [Space(padA), Header("--�p�g���[���֘A--")]
    NavMeshPatrol patrol;
    [SerializeField, Header("�ړI�n��"), ReadOnly]
    int targetNum;
    [SerializeField, Header("���̖ړI�n�̓Y����"), ReadOnly]
    int targetIndex = 0;

    // �A�j���[�^�[
    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        // �e��R���|�[�l���g�擾
        patrol = GetComponent<NavMeshPatrol>();
        animator = GetComponent<Animator>();
        targetNum = patrol.GetTargets().Length; // �ړI�n���̎擾
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
            patrol.SetAgentParam(trackingParam, true);
            state = State.Tracking;
            return;
        }

        if (cnt > waitInterval)
        {
            cnt = 0.0f;
            patrol.SetAgentParam(moveParam);
            patrol.ExcutePatrol(targetIndex);
            state = State.Move;
        }
    }

    protected override void MoveFunc()
    {
        SubPressure(moveParam.consumePressure * Time.deltaTime);
        if(currentPressure <= healParam.startHealLine)
        {
            state = State.Heal;
            patrol.Stop();
            return;
        }

        if (FindPlayerAtFOV().isFind)
        {
            state = State.Tracking;
            patrol.SetAgentParam(trackingParam, true);
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
        // �ړ��������܂��c���Ă���Ȃ爳�͂�����
        if(patrol.GetRemainingDistance() > 0.0f) SubPressure(trackingParam.consumePressure * Time.deltaTime);

        if (currentPressure <= healParam.startHealLine)
        {
            state = State.Heal;
            patrol.Stop();
            return;
        }

        (bool isFind, float distance) = FindPlayerAtFOV();
        if (!isFind)
        {
            state = State.Idle;
            return;
        }

        // �v���C���[��������������Ă����ꍇ�A���S�Ɉڍs����
        if(Vector3.Dot(eyeTransform.forward, PlayerController.instance.transform.Find("HorizontalRotationShaft").Find("MoveShaft").transform.forward) < -0.75f)
        {
            state = State.Escape;
            patrol.Stop();
            patrol.SetAgentParam(escapeParam);
            return;
        }

        // �ړI�n�̎Z�o
        Vector3 targetVector = transform.position - target.transform.position;
        targetVector.Normalize();
        Vector3 Position = target.transform.position + targetVector * trackingKeepDistance;
        patrol.ExcuteCustom(Position);
    }

    protected override void EscapeFunc()
    {
        SubPressure(escapeParam.consumePressure * Time.deltaTime);
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

    protected override void HealFunc()
    {
        base.HealFunc();
        if(currentPressure >= healParam.endHealLine)
        {
            state = State.Idle;
        }
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
