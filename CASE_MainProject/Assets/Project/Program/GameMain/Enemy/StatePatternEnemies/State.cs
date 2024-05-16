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

    public virtual void CollisionEnter(Collision collision)
    {
        // �ՓˊJ�n����
    }

    public virtual void CollisionStay(Collision collision)
    {
        // �Փ˒�����
    }

    public virtual void CollisionExit(Collision collision)
    {
        // �ՓˏI������
    }

    public virtual void TriggerEnter(Collider collider)
    {
        // �ՓˊJ�n����
    }

    public virtual void TriggerStay(Collider collider)
    {
        // �Փ˒�����
    }

    public virtual void TriggerExit(Collider collider)
    {
        // �ՓˏI������
    }
}
