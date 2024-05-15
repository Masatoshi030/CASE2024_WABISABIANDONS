using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    [SerializeField, Header("�X�e�[�g�̎Q�Ɛ�I�u�W�F�N�g")]
    GameObject stateObject;

    Enemy enemy;
    public Enemy Controller { get => enemy; set => enemy = value; }

    [SerializeField, Header("���݂̏��"), ReadOnly]
    EnemyState currentState;

    EnemyState nextState;
    bool bNextState = false;

    [SerializeField, Header("��Ԉꗗ")]
    EnemyState[] states;
    [SerializeField, Header("��Ԗ��ꗗ"), ReadOnly]
    string[] stateNames;

    [SerializeField, Header("�o�ߎ���"), ReadOnly]
    float cnt;
    public float Cnt { get => cnt; }

    Dictionary<string, EnemyState> stateList = new Dictionary<string, EnemyState>();


    public void Initialize()
    {
        int num = 0;
        for(int i = 0; i < stateObject.GetComponentCount(); i++)
        {
            if (stateObject.GetComponentAtIndex(i) is EnemyState)
            {
                num++;
            }
        }

        states = new EnemyState[num];
        num = 0;
        for (int i = 0; i < stateObject.GetComponentCount(); i++)
        {
            if (stateObject.GetComponentAtIndex(i) is EnemyState)
            {
                states[num] = (EnemyState)stateObject.GetComponentAtIndex(i);
                num++;
            }
        }

        stateNames = new string[states.Length];
        // ���X�g�̍쐬
        for(int i = 0; i < states.Length; i++)
        {
            stateNames[i] = states[i].StateName;
            stateList.Add(stateNames[i] ,states[i]);
            states[i].Controller = enemy;
            states[i].Machine = this;
            // ������
            states[i].Initialize();
        }
        // �����X�e�[�g�͔z���0�Ԗ�
        currentState = states[0];
        currentState.Enter();
        enemy.StateName = stateNames[0];
    }

    public void MainFunc()
    {
        if(currentState != null)
        {
            Debug.Log(enemy.name);
            cnt += Time.deltaTime;
            currentState.MainFunc();
        }
        
        if(bNextState)
        {
            cnt = 0.0f;
            currentState.Exit();
            currentState = nextState;
            bNextState = false;
            currentState.Enter();
        }
    }

    public void TransitionTo(string key)
    {
        if(stateList.ContainsKey(key))
        {
            bNextState = true;
            nextState = stateList[key];
            enemy.StateName = key;
        }
        else
        {
            Debug.Log("�X�e�[�g�X�V�G���[ : " + enemy.name + "(stateName : " + nextState + ")");
        }
    }
}
