using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateMachine : StateMachine
{
    Enemy enemy;
    public Enemy EnemyComponent { get => enemy; set => enemy = value; }

    public override void Initialize()
    {
        // �X�e�[�g���̎擾
        int num = 0;
        for(int i = 0; i < stateObject.GetComponentCount(); i++)
        {
            // �G�l�~�[�X�e�[�g�̌p���Ȃ�J�E���g�𑝂₷
            if (stateObject.GetComponentAtIndex(i) is EnemyState)
            {
                num++;
            }
        }

        // �擾�ɐ��������X�e�[�g���z������
        states = new EnemyState[num];
        stateNames = new string[num];
        num = 0;
        // ��ԁA��Ԗ��A�������쐬
        for (int i = 0; i < stateObject.GetComponentCount(); i++)
        {
            if (stateObject.GetComponentAtIndex(i) is EnemyState)
            {
                // �X�e�[�g�̊i�[
                EnemyState state = (EnemyState)stateObject.GetComponentAtIndex(i);
                state.EnemyObject = enemy;
                states[num] = state;
                stateNames[num] = states[num].StateName;
                states[num].Machine = this;
                states[num].Initialize();
                stateList.Add(stateNames[num], states[num]);
                num++;
            }
        }
        // �����X�e�[�g�͔z���0�Ԗ�
        currentState = states[0];
        currentState.Enter();
        enemy.StateName = stateNames[0];
    }

    public override bool TransitionTo(string key)
    {
        if(enemy.Hp <= 0.0f)
        {
            base.TransitionTo("���S");
            return false;
        }

        bool b = base.TransitionTo(key);
        // �����ɃL�[���o�^����Ă��邩�`�F�b�N
        if (b) { enemy.StateName = key; return true; }
        else return false;
    }
}
