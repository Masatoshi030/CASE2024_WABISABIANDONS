using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy_Mob : Enemy_Parent
{
    public enum State
    {
        [InspectorName("�ҋ@")] Idle,
        [InspectorName("�ړ�")] Move,
        [InspectorName("�ǔ�")] Tracking,
        [InspectorName("����")] Escape,
        [InspectorName("�U��A")] AttackA,
        [InspectorName("�U��B")] AttackB,
        [InspectorName("����A")]SpecialA,
        [InspectorName("����B")]SpecialB,
        [InspectorName("��")] Heal,
        [InspectorName("�j��")] Death,
    }

    [SerializeField, Header("�G���F����")]
    protected float viewingDistance = 20.0f;

    [SerializeField, Header("����p")]
    protected float viewingAngle = 80.0f;

    [SerializeField, Header("���_�̍���")]
    protected float eyeHeight = 1.0f;

    [SerializeField, Header("���"), Toolbar(typeof(State))]
    protected State state = State.Idle;
    
    protected void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected void Update()
    {
        base.Update();
        switch (state)
        {
            case State.Idle:IdleFunc(); break;
            case State.Move: MoveFunc(); break;
            case State.Tracking: TrackingFunc(); break;
            case State.Escape: EscapeFunc(); break;
            case State.AttackA: AttackFunc1(); break;
            case State.AttackB: AttackFunc2(); break;
            case State.SpecialA: SpecialFuncA(); break;
            case State.SpecialB: SpecialFuncB(); break;
            case State.Heal: HealFunc(); break;
            case State.Death: break;
        }
    }

    /*
     * <summary>
     * �ҋ@��Ԋ֐�
     * <param>
     * �Ȃ�
     * <return>
     * �Ȃ�
     */
    protected abstract void IdleFunc();

    /*
     * <summary>
     * �ړ���Ԋ֐�
     * <param>
     * �Ȃ�
     * <return>
     * �Ȃ�
     */
    protected abstract void MoveFunc();

    /*
     * <summary>
     * �ǔ���Ԋ֐�
     * <param>
     * �Ȃ�
     * <return>
     * �Ȃ�
     */
    protected abstract void TrackingFunc();

    /*
     * <summary>
     * ������Ԋ֐�
     * <param>
     * �Ȃ�
     * <return>
     * �Ȃ�
     */
    protected abstract void EscapeFunc();

    /*
     * <summary>
     * �U���֐��p�^�[��A
     * <param>
     * �Ȃ�
     * <return>
     * �Ȃ�
     */
    protected abstract void AttackFunc1();

    /*
     * <summary>
     * �U���֐��p�^�[��B
     * <param>
     * �Ȃ�
     * <return>
     * �Ȃ�
     */
    protected abstract void AttackFunc2();

    /*
     * <summary>
     * �U���֐��p�^�[��B
     * <param>
     * �Ȃ�
     * <return>
     * �Ȃ�
     */
    protected abstract void SpecialFuncA();

    /*
     * <summary>
     * �U���֐��p�^�[��B
     * <param>
     * �Ȃ�
     * <return>
     * �Ȃ�
     */
    protected abstract void SpecialFuncB();

    /*
     * <summary>
     * �񕜏�Ԋ֐�
     * <param>
     * �Ȃ�
     * <return>
     * �Ȃ�
     */
    protected abstract void HealFunc();

    /*
     * <summary>
     * ���S��Ԋ֐�
     * <param>
     * �Ȃ�
     * <return>
     * �Ȃ�
     */
    protected abstract void DeathFunc();

    protected override void DestroyFunc()
    {

    }

    /*
     * <summary>
     * �v���C���[������p�����ɒT������
     * <param>
     * �Ȃ�
     * <returns>
     * bool isFind, float distance
     */
    protected (bool isFind, float distance) FindPlayerAtFOV()
    {
        // �����𑪂�
        float distance = Vector3.Distance(target.transform.position, transform.position);

        if (distance < viewingDistance)
        {
            // ��������^�[�Q�b�g�Ɍ������x�N�g��
            Vector3 MetoTarget = target.transform.position - transform.position;
            // �^�[�Q�b�g���玩���Ɍ������x�N�g��
            Vector3 TargettoMe = -MetoTarget;
            // �O�ς�y�����琳�ʂ̍��E�ǂ���ɂ��邩���߂�
            Vector3 axis = Vector3.Cross(transform.forward, MetoTarget);

            // �p�x��+�Ȃ琳�ʂ��E�ɂ���-�Ȃ獶�ɂ���
            float angle = Vector3.Angle(transform.forward, MetoTarget) * (axis.y < 0 ? -1.0f : 1.0f);

            if (Mathf.Abs(angle) < viewingAngle / 2)
            {
                Vector3 Direction = MetoTarget.normalized;
                // �ڂ̈ʒu�����߂�
                Vector3 eyePos = transform.position + new Vector3(0.0f, eyeHeight, 0.0f);
                // ��̍쐬�ƕ\��
                Ray ray = new Ray(eyePos, Direction);
                Debug.DrawRay(eyePos, Direction, Color.red);
                RaycastHit[] hits = Physics.RaycastAll(eyePos, Direction, viewingDistance);
                if (hits.Length > 0)
                {
                    if (hits[0].transform.tag == "Player")
                    {
                        return (true, distance);
                    }
                }
            }
        }
        return (false, distance);
    }
}
