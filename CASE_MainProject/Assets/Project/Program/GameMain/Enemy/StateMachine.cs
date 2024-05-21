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
            nextState = stateList[key];
            cnt = 0.0f;
            currentState.Exit();
            currentState = nextState;
            nextState = null;
            currentState.Enter();
            return true;
        }
        else
        {
            Debug.Log("�X�e�[�g�X�V�G���[ : " + controller.name + "( stateName : " + nextState.StateName + " )");
            return false;
        }
    }

    public virtual void TransitionTo(int index)
    {
        if (index >= states.Length)
        {
            index %= states.Length;
        }
        nextState = states[index];
        cnt = 0.0f;
        currentState.Exit();
        currentState = nextState;
        currentState.Enter();
        nextState = null;
    }

    public virtual void CollisionEnterSelf(GameObject other)
    {
        if (currentState != null) currentState.CollisionEnterSelf(other);
    }

    public virtual void CollisionEnterOpponent(GameObject other)
    {
        if (currentState != null) currentState.CollisionEnterOpponent(other);
    }

    public virtual void CollisionStaySelf(GameObject other)
    {
        if (currentState != null) currentState.CollisionStaySelf(other);
    }

    public virtual void CollisionStayOpponent(GameObject other)
    {
        if (currentState != null) currentState.CollisionStayOpponent(other);
    }

    public virtual void CollisionExitSelf(GameObject other)
    {
        if (currentState != null) currentState.CollisionExitSelf(other);
    }

    public virtual void CollisionExitOpponent(GameObject other)
    {
        if (currentState != null) currentState.CollisionExitOpponent(other);
    }

    public virtual void TriggerEnterSelf(GameObject other)
    {
        if (currentState != null) currentState.TriggerEnterSelf(other);
    }

    public virtual void TriggerEnterOpponent(GameObject other)
    {
        if (currentState != null) currentState.TriggerEnterOpponent(other);
    }

    public virtual void TriggerStaySelf(GameObject other)
    {
        if (currentState != null) currentState.TriggerStaySelf(other);
    }

    public virtual void TriggerStayOpponent(GameObject other)
    {
        if (currentState != null) currentState.TriggerStayOpponent(other);
    }

    public virtual void TriggerExitSelf(GameObject other)
    {
        if (currentState != null) currentState.TriggerExitSelf(other);
    }

    public virtual void TriggerExitOpponent(GameObject other)
    {
        if (currentState != null) currentState.TriggerExitOpponent(other);
    }
}
