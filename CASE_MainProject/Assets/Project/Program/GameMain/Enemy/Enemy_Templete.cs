using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �G�e���v���[�g

public class Enemy_Templete : Enemy_Mob
{
    // �C���X�y�N�^�[�����₷���悤�Ɋ֘A���Ƃɕ�����̂�����
    [Space(padA), Header("--���C���p�����[�^--")]
    [SerializeField, Header("�p�����[�^(��)")]
    float param;

    [Space(padA), Header("--�C���^�[�o��--")]
    [SerializeField, Header("�C���^�[�o��(��)")]
    float interval;

    [Space(padA), Header("--�U���֘A--")]
    [SerializeField, Header("�U��(��)")]
    float attackPower;

    // Start is called before the first frame update
    void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
    }

    /*
    * <summary>
    * �ҋ@��Ԋ֐�
    * <param>
    * void
    * <return>
    * void
    */
    protected override void IdleFunc()
    {
        throw new System.NotImplementedException();
    }

    /*
    * <summary>
    * �ړ���Ԋ֐�
    * <param>
    * void
    * <return>
    * void
    */
    protected override void MoveFunc()
    {
        throw new System.NotImplementedException();
    }

    /*
    * <summary>
    * �ǔ���Ԋ֐�
    * <param>
    * void
    * <return>
    * void
    */
    protected override void TrackingFunc()
    {
        throw new System.NotImplementedException();
    }

    /*
    * <summary>
    * ���S��Ԋ֐�
    * <param>
    * void
    * <return>
    * void
    */
    protected override void EscapeFunc()
    {
        throw new System.NotImplementedException();
    }

    /*
    * <summary>
    * �U���֐�A
    * <param>
    * void
    * <return>
    * void
    */
    protected override void AttackFuncA()
    {
        throw new System.NotImplementedException();
    }

    /*
    * <summary>
    * �U���֐�B
    * <param>
    * void
    * <return>
    * void
    */
    protected override void AttackFuncB()
    {
        throw new System.NotImplementedException();
    }

    /*
    * <summary>
    * ����֐�A
    * <param>
    * void
    * <return>
    * void
    */
    protected override void SpecialFuncA()
    {
        throw new System.NotImplementedException();
    }

    /*
    * <summary>
    * ����֐�B
    * <param>
    * void
    * <return>
    * void
    */
    protected override void SpecialFuncB()
    {
        throw new System.NotImplementedException();
    }

    /*
    * <summary>
    * �񕜊֐�
    * <param>
    * void
    * <return>
    * void
    */
    protected override void HealFunc()
    {
        throw new System.NotImplementedException();
    }

    /*
    * <summary>
    * ���S��Ԋ֐�
    * <param>
    * void
    * <return>
    * void
    */
    protected override void DeathFunc()
    {
        throw new System.NotImplementedException();
    }

    /*
    * <summary>
    * �j�󎞌Ăяo���֐�
    * <param>
    * void
    * <return>
    * void
    */
    protected override void DestroyFunc()
    {
        base.DestroyFunc();
    }
}
