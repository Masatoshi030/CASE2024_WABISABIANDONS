using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState : MonoBehaviour
{
    // Enemy�R���|�[�l���g
    protected Enemy enemy;
    public Enemy Controller { get => enemy; set => enemy = value; }

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
}