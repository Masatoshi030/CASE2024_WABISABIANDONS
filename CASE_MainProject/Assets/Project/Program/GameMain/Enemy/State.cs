using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State : MonoBehaviour
{
    protected const float pad = 10.0f;

    GameObject controller;
    public GameObject Controller { get => controller; set => controller = value; }
    // �X�e�[�g�}�V���R���|�[�l���g
    protected StateMachine machine;
    public StateMachine Machine { get => machine; set => machine = value; }

    [SerializeField, Header("��Ԗ�")]
    string stateName;
    public string StateName { get => stateName; }

    public virtual void Initialize()
    {
        // ����������
    }

    public virtual void Enter()
    {
        // �J�n����
    }

    public virtual void MainFunc()
    {
        // �X�V����
    }

    public virtual void Exit()
    {
        // �I��������
    }

    public virtual void CollisionEnterSelf(GameObject other)
    {
        // �ՓˊJ�n����(�Z���t�Ăяo��)
    }

    public virtual void CollisionEnterOpponent(GameObject other)
    {
        // �ՓˊJ�n����(����Ăяo��)
    }

    public virtual void CollisionStaySelf(GameObject other)
    {
        // �Փ˒�����(�Z���t�Ăяo��)
    }

    public virtual void CollisionStayOpponent(GameObject other)
    {
        // �Փ˒�����(����Ăяo��)
    }

    public virtual void CollisionExitSelf(GameObject other)
    {
        // �ՓˏI������(�Z���t�Ăяo��)
    }

    public virtual void CollisionExitOpponent(GameObject other)
    {
        // �ՓˏI������(����Ăяo��)
    }

    public virtual void TriggerEnterSelf(GameObject other)
    {
        // �ՓˊJ�n����(�Z���t�Ăяo��)
    }

    public virtual void TriggerEnterOpponent(GameObject other)
    {
        // �ՓˊJ�n����(����Ăяo��)
    }

    public virtual void TriggerStaySelf(GameObject other)
    {
        // �Փ˒�����(�Z���t�Ăяo��)
    }

    public virtual void TriggerStayOpponent(GameObject other)
    {
        // �Փ˒�����(����Ăяo��)
    }

    public virtual void TriggerExitSelf(GameObject other)
    {
        // �ՓˏI������(�Z���t�Ăяo��)
    }

    public virtual void TriggerExitOpponent(GameObject other)
    {
        // �ՓˏI������(����Ăяo��)
    }
}
