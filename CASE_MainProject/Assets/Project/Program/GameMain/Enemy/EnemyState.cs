using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState : State
{
    // Enemy�R���|�[�l���g
    protected Enemy enemy;
    public Enemy EnemyObject { get => enemy; set => enemy = value; }
    [SerializeField, Header("�A�j���[�V���������邩")]
    bool isAnimation = true;
    [SerializeField, Header("�J�n���A�j���[�V����")]
    protected string enterAnimation;
    [SerializeField, Header("�A�j���[�V�������x")]
    protected float animSpeed = 1.0f;
    [SerializeField, Header("�g�p���C��")]
    protected float pressureAmount;
    [SerializeField, Header("-���͈��ȉ��̑J��ID-")]
    protected int pressureZeroID = 1;

    public override void Initialize()
    {
        base.Initialize();
    }

    /*
     * <summary>
     * �J�n������
     * <param>
     * void
     * <return>
     * void
     */
    public override void Enter()
    {
        base.Enter();
        // �A�j���[�V�����̑��x�Ǝw�肵���A�j���[�V�����̊J�n
        if(isAnimation)
        {
            enemy.EnemyAnimator.enabled = true;
            enemy.EnemyAnimator.speed = animSpeed;
            enemy.EnemyAnimator.Play(enterAnimation);
        }
        else
        {
            enemy.EnemyAnimator.enabled = false;
        }
    }

    public override void MainFunc()
    {
        base.MainFunc();

        enemy.CalcPressure(-pressureAmount * Time.deltaTime);
        if(enemy.Pressre <= 0.0)
        {
            machine.IsUpdate = false;
            machine.TransitionTo(pressureZeroID);
        }
    }
}