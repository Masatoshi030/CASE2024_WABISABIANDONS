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
    [SerializeField, Header("���"), Toolbar(typeof(State))]
    State state = State.Idle;
    
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
}
