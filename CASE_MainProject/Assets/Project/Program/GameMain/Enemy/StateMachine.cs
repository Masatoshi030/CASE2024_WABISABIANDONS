using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    [System.Serializable]
    public class StateData
    {
        [SerializeField, Header("ID")]
        public int id;
        [SerializeField, Header("��Ԗ�")]
        public string name;
        [SerializeField, Header("�X�e�[�g")]
        public State state;
    }


    // ���̃R���|�[�l���g���������Ă�I�u�W�F�N�g
    protected GameObject controller;
    public GameObject Controller { get => controller; set => controller = value; }

    [SerializeField, Header("�X�e�[�g�̎Q�Ɛ�I�u�W�F�N�g")]
    protected GameObject stateObject;
    public GameObject StateObject { get => stateObject; }

    [SerializeField, Header("�������ID")]
    protected int initStateID = 1;

    [SerializeField, Header("���݂̏��"), ReadOnly]
    protected StateData currentState;
    protected StateData nextState;

    // ��ԃ��X�g
    [SerializeField, Header("��ԃf�[�^")]
    protected List<StateData> stateDatas = new List<StateData>();
    public List<StateData> StateDatas { get => stateDatas; }

    // ID���X�g
    protected List<int> idDatas = new List<int>();
    public List<int> IDDatas { get => idDatas; }

    [SerializeField, Header("�o�ߎ���"), ReadOnly]
    float cnt;
    public float Cnt { get => cnt; }

    bool bTransition = false;

    /*
     * <summary>
     * ����������
     * <param>
     * void
     * <return>
     * void
     */
    public virtual void Initialize()
    {
        int num = 0;
        // ��ԁA��Ԗ��A�������쐬
        for (int i = 0; i < stateObject.GetComponentCount(); i++)
        {
            if (stateObject.GetComponentAtIndex(i) is State)
            {
                // �X�e�[�g�̊i�[
                State state = (State)stateObject.GetComponentAtIndex(i);
                AddState(state);
                num++;
            }
        }
        // �����X�e�[�g��ݒ�
        currentState = stateDatas[initStateID];
        currentState.state.Enter();
    }

    /*
     * <summary>
     * ���C������
     * <param>
     * void
     * <return>
     * void
     */
    public virtual void MainFunc()
    {
        if(bTransition)
        {
            cnt = 0.0f;
            currentState.state.Exit();
            currentState = nextState;
            nextState = null;
            bTransition = false;
            currentState.state.Enter();
        }
        if (currentState != null)
        {
            cnt += Time.deltaTime;
            // ���݂̏�Ԃ̃��C���������Ă�
            currentState.state.MainFunc();
        }
    }

    /*
     * <summary>
     * �J�ڏ���
     * <param>
     * int �J�ڐ�ID
     * <return>
     * bool �J�ڂ̐���
     */
    public virtual bool TransitionTo(int id)
    {
        if(!bTransition)
        {
            if(idDatas.Contains(id))
            {
                nextState = stateDatas[id];
                bTransition = true;
                return true;
            }
            else
            {
                Debug.Log(controller.name + " �X�e�[�g�X�V�G���[ StateID : " + id);
                return false;
            }
        }
        return false;
    }

    public virtual bool TransitionTo(State state)
    {
        if(!bTransition)
        {
            nextState.state = state;
            nextState.id = state.StateID;
            nextState.name = state.name;
            bTransition = true;
            return true;
        }
        else
        {
            return false;
        }
    }

    public virtual void AddState(State state)
    {
        StateData data = new StateData();
        data.state = state;
        data.id = state.StateID;
        data.name = state.StateName;
        data.state.Machine = this;
        state.Initialize();
    }

    /*
     * <summary>
     * �Փ˂̏���(���g�ŌĂяo��)
     * <param>
     * GameObject �Փ˃I�u�W�F�N�g
     * <return>
     * void
     */
    public virtual void CollisionEnterSelf(Collision collision)
    {
        if (currentState != null) currentState.state.CollisionEnterSelf(collision);
    }

    /*
     * <summary>
     * �Փ˂̏���(���葤�Ăяo��)
     * <param>
     * GameObject �Փ˃I�u�W�F�N�g
     * <return>
     * void
     */
    public virtual void CollisionEnterOpponent(Collision collision)
    {
        if (currentState != null) currentState.state.CollisionEnterOpponent(collision);
    }

    public virtual void CollisionStaySelf(Collision collision)
    {
        if (currentState != null) currentState.state.CollisionStaySelf(collision);
    }

    public virtual void CollisionStayOpponent(Collision collision)
    {
        if (currentState != null) currentState.state.CollisionStayOpponent(collision);
    }

    public virtual void CollisionExitSelf(Collision collision)
    {
        if (currentState != null) currentState.state.CollisionExitSelf(collision);
    }

    public virtual void CollisionExitOpponent(Collision collision)
    {
        if (currentState != null) currentState.state.CollisionExitOpponent(collision);
    }

    public virtual void TriggerEnterSelf(Collider other)
    {
        if (currentState != null) currentState.state.TriggerEnterSelf(other);
    }

    public virtual void TriggerEnterOpponent(Collider other)
    {
        if (currentState != null) currentState.state.TriggerEnterOpponent(other);
    }

    public virtual void TriggerStaySelf(Collider other)
    {
        if (currentState != null) currentState.state.TriggerStaySelf(other);
    }

    public virtual void TriggerStayOpponent(Collider other)
    {
        if (currentState != null) currentState.state.TriggerStayOpponent(other);
    }

    public virtual void TriggerExitSelf(Collider other)
    {
        if (currentState != null) currentState.state.TriggerExitSelf(other);
    }

    public virtual void TriggerExitOpponent(Collider other)
    {
        if (currentState != null) currentState.state.TriggerExitOpponent(other);
    }
}
