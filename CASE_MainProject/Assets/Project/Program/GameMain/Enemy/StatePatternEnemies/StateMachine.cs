using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    // ���̃R���|�[�l���g���������Ă�I�u�W�F�N�g
    protected GameObject controller;
    public GameObject Controller { get => controller; set => controller = value; }

    [SerializeField, Header("�X�e�[�g�̎Q�Ɛ�I�u�W�F�N�g")]
    protected GameObject stateObject;

    [SerializeField, Header("���݂̏��"), ReadOnly]
    protected State currentState;

    protected State nextState;
    protected bool bNextState = false;

    [SerializeField, Header("��Ԉꗗ")]
    protected State[] states;
    [SerializeField, Header("��Ԗ��ꗗ"), ReadOnly]
    protected string[] stateNames;

    protected Dictionary<string, State> stateList = new Dictionary<string, State>();

    [SerializeField, Header("�o�ߎ���"), ReadOnly]
    float cnt;
    public float Cnt { get => cnt; }

    public virtual void Initialize()
    {
        // �X�e�[�g���̎擾
        int num = 0;
        for (int i = 0; i < stateObject.GetComponentCount(); i++)
        {
            // �G�l�~�[�X�e�[�g�̌p���Ȃ�J�E���g�𑝂₷
            if (stateObject.GetComponentAtIndex(i) is State)
            {
                num++;
            }
        }

        // �擾�ɐ��������X�e�[�g���z������
        states = new State[num];
        stateNames = new string[num];
        num = 0;
        // ��ԁA��Ԗ��A�������쐬
        for (int i = 0; i < stateObject.GetComponentCount(); i++)
        {
            if (stateObject.GetComponentAtIndex(i) is State)
            {
                // �X�e�[�g�̊i�[
                states[num] = (State)stateObject.GetComponentAtIndex(i);
                stateNames[num] = states[num].StateName;
                states[num].Controller = controller;
                states[num].Machine = this;
                states[num].Initialize();
                stateList.Add(stateNames[num], states[num]);
                num++;
            }
        }
        // �����X�e�[�g�͔z���0�Ԗ�
        currentState = states[0];
        currentState.Enter();
    }

    public virtual void MainFunc()
    {
        // ��Ԃ̕ύX�t���O��true�Ȃ玟�̏�Ԃɐ؂�ւ���
        if (bNextState)
        {
            cnt = 0.0f;
            currentState.Exit();
            currentState = nextState;
            bNextState = false;
            currentState.Enter();
        }
        if (currentState != null)
        {
            cnt += Time.deltaTime;
            // ���݂̏�Ԃ̃��C���������Ă�
            currentState.MainFunc();
        }
    }

    public virtual bool TransitionTo(string key)
    {
        // �����ɃL�[���o�^����Ă��邩�`�F�b�N
        if (stateList.ContainsKey(key))
        {
            bNextState = true;
            nextState = stateList[key];
            return true;
        }
        else
        {
            Debug.Log("�X�e�[�g�X�V�G���[ : " + controller.name + "( stateName : " + nextState.StateName + " )");
            return false;
        }
    }

    public virtual void CollisionEnter(Collision collision)
    {
        if (currentState != null) currentState.CollisionEnter(collision);
    }

    public virtual void CollisionStay(Collision collision)
    {
        if (currentState != null) currentState.CollisionStay(collision);
    }

    public virtual void CollisionExit(Collision collision)
    {
        if (currentState != null) currentState.CollisionExit(collision);
    }

    public virtual void TriggerEnter(Collider collider)
    {
        if (currentState != null) currentState.TriggerEnter(collider);
    }

    public virtual void TriggerStay(Collider collider)
    {
        if (currentState != null) currentState.TriggerStay(collider);
    }

    public virtual void TriggerExit(Collider collider)
    {
        if (currentState != null) currentState.TriggerExit(collider);
    }
}
