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
            currentState.Exit();
            currentState = nextState;
            nextState = null;
            currentState.Enter();
            bTransition = false;
        }
        if (currentState != null)
        {
            cnt += Time.deltaTime;
            // ���݂̏�Ԃ̃��C���������Ă�
            currentState.MainFunc();
        }
    }

    /*
     * <summary>
     * �J�ڏ���
     * <param>
     * string �J�ڐ於��
     * <return>
     * bool �J�ڂ̐���
     */
    public virtual bool TransitionTo(string key)
    {
        if(!bTransition)
        {
            // �����ɃL�[���o�^����Ă��邩�`�F�b�N
            if (stateList.ContainsKey(key))
            {
                nextState = stateList[key];
                // �J�ڗ\��
                bTransition = true;
                return true;
            }
            else
            {
                Debug.Log("�X�e�[�g�X�V�G���[ : " + controller.name + "( stateName : " + key + " )");
                return false;
            }
        }
        return false;
    }

    /*
     * <summary>
     * �J�ڏ���
     * <param>
     * int �J�ڐ�̃C���f�b�N�X
     * <return>
     * bool �J�ڂ̐���
     */
    public virtual bool TransitionTo(int index)
    {
        if (index >= states.Length || bTransition)
        {
            return false;
        }
        nextState = states[index];
        // �J�ڗ\��
        bTransition = true;
        return true;
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
        if (currentState != null) currentState.CollisionEnterSelf(collision);
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
        if (currentState != null) currentState.CollisionEnterOpponent(collision);
    }

    public virtual void CollisionStaySelf(Collision collision)
    {
        if (currentState != null) currentState.CollisionStaySelf(collision);
    }

    public virtual void CollisionStayOpponent(Collision collision)
    {
        if (currentState != null) currentState.CollisionStayOpponent(collision);
    }

    public virtual void CollisionExitSelf(Collision collision)
    {
        if (currentState != null) currentState.CollisionExitSelf(collision);
    }

    public virtual void CollisionExitOpponent(Collision collision)
    {
        if (currentState != null) currentState.CollisionExitOpponent(collision);
    }

    public virtual void TriggerEnterSelf(Collider other)
    {
        if (currentState != null) currentState.TriggerEnterSelf(other);
    }

    public virtual void TriggerEnterOpponent(Collider other)
    {
        if (currentState != null) currentState.TriggerEnterOpponent(other);
    }

    public virtual void TriggerStaySelf(Collider other)
    {
        if (currentState != null) currentState.TriggerStaySelf(other);
    }

    public virtual void TriggerStayOpponent(Collider other)
    {
        if (currentState != null) currentState.TriggerStayOpponent(other);
    }

    public virtual void TriggerExitSelf(Collider other)
    {
        if (currentState != null) currentState.TriggerExitSelf(other);
    }

    public virtual void TriggerExitOpponent(Collider other)
    {
        if (currentState != null) currentState.TriggerExitOpponent(other);
    }
}
