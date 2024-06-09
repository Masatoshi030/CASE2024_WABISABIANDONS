using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State : MonoBehaviour
{
    protected const float pad = 10.0f;

    protected bool continueProcessing = true;

    GameObject controller;
    public GameObject Controller { get => controller; set => controller = value; }
    // �X�e�[�g�}�V���R���|�[�l���g
    protected StateMachine machine;
    public StateMachine Machine { get => machine; set => machine = value; }

    [SerializeField, Header("��Ԗ�")]
    string stateName;
    public string StateName { get => stateName; set => stateName = value; }

    [SerializeField, Header("���ID")]
    int stateID;
    public int StateID { get => stateID; }

    public virtual void Initialize()
    {
        // ����������
    }

    public virtual void Enter()
    {
        continueProcessing = true;
        // �J�n����
    }

    public virtual void MainFunc()
    {
        // �X�V����
    }

    public virtual void Exit()
    {
        continueProcessing = false;
        // �I��������
    }

    public virtual void CollisionEnterSelf(Collision collision)
    {
        // �ՓˊJ�n����(�Z���t�Ăяo��)
    }

    public virtual void CollisionEnterOpponent(Collision collision)
    {
        // �ՓˊJ�n����(����Ăяo��)
    }

    public virtual void CollisionStaySelf(Collision collision)
    {
        // �Փ˒�����(�Z���t�Ăяo��)
    }

    public virtual void CollisionStayOpponent(Collision collision)
    {
        // �Փ˒�����(����Ăяo��)
    }

    public virtual void CollisionExitSelf(Collision collision)
    {
        // �ՓˏI������(�Z���t�Ăяo��)
    }

    public virtual void CollisionExitOpponent(Collision collision)
    {
        // �ՓˏI������(����Ăяo��)
    }

    public virtual void TriggerEnterSelf(Collider other)
    {
        // �ՓˊJ�n����(�Z���t�Ăяo��)
    }

    public virtual void TriggerEnterOpponent(Collider other)
    {
        // �ՓˊJ�n����(����Ăяo��)
    }

    public virtual void TriggerStaySelf(Collider other)
    {
        // �Փ˒�����(�Z���t�Ăяo��)
    }

    public virtual void TriggerStayOpponent(Collider other)
    {
        // �Փ˒�����(����Ăяo��)
    }

    public virtual void TriggerExitSelf(Collider other)
    {
        // �ՓˏI������(�Z���t�Ăяo��)
    }

    public virtual void TriggerExitOpponent(Collider other)
    {
        // �ՓˏI������(����Ăяo��)
    }
}
