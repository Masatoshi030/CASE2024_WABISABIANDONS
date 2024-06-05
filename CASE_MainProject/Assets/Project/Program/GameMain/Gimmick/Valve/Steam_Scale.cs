using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Steam_Scale : Subscriber
{
    enum State
    {
        WaitStart, RunningToEnd, End, RunningToStart,
    }

    [SerializeField, Header("���"), Toolbar(typeof(State))]
    State state = State.WaitStart;

    [SerializeField, Header("���ˑ��x")]
    float steam_Speed;

    [SerializeField, Header("�ő啬�ˋ���")]
    float steam_Scale;

    [SerializeField, Header("���ݕ��ˋ���"), ReadOnly]
    float now_Scale;

    [SerializeField, Header("���Ԍo�߂���")]
    bool steam_Timer = true;

    [SerializeField, Header("���쎞��")]
    float steam_Limit;

    private float nothing_Steam = 0;  //�X�`�[�����o�ĂȂ����

    private bool move_Valve;  //�쓮�������H
    private Valve_Base.Valve_Type type;  //�o���u�̎��
    Vector3 change_localScale;  //�ύX����X�P�[�����ꎞ�⊮����
    private float limit;  //�������Ԃ𑪂�

    void Start()
    {
        switch (type)
        {
            case Valve_Base.Valve_Type.open:
                change_localScale = transform.localScale;
                change_localScale.y = nothing_Steam;
                Debug.Log("0�ɐݒ�");
                break;

            case Valve_Base.Valve_Type.close:
                change_localScale = transform.localScale;
                change_localScale.y = steam_Scale; //�X�`�[�����o�Ă�����
                Debug.Log("�ő�l�̐ݒ�");
                break;
        }

        //�傫���̕ύX���i�[
        this.transform.localScale = change_localScale;
    }

    // Update is called once per frame
    void Update()
    {
        //���
        switch (state)
        {
            //�����O
            case State.WaitStart: 
                if (move_Valve)
                { state = State.RunningToEnd; }  //����
                move_Valve = false;
                break;

            //������
            case State.RunningToEnd:
                switch (type)
                    {
                        case Valve_Base.Valve_Type.open: //�o���u���J����
                        move_Open();
                        if(now_Scale>=steam_Scale)
                        {
                            stop_Open();
                            state = State.End;
                        }
                        break;

                        //�o���u��߂�
                        case Valve_Base.Valve_Type.close:
                            move_Close();
                        if (now_Scale <= nothing_Steam)
                        {
                            stop_Close();
                            state = State.End;
                        }
                        break;
                    }
                break;
            //�����O�ɖ߂�
            case State.RunningToStart:
            
                switch (type)
                {
                    //�o���u���J����
                    case Valve_Base.Valve_Type.open:
                        move_Close();
                        if (now_Scale <= nothing_Steam)
                        {
                            stop_Close();
                            state = State.WaitStart;
                        }
                        break;

                    //�o���u��߂�
                    case Valve_Base.Valve_Type.close:
                        move_Open();
                        if (now_Scale >= steam_Scale)
                        {
                            stop_Open();
                            state = State.WaitStart;
                        }
                        break;
                }

                break;

            //������
            case State.End:                
               
                if(steam_Timer)
                {
                    limit += Time.deltaTime;

                    if (limit > steam_Limit)
                    {
                        state = State.RunningToStart;
                        limit = 0;
                    }
                }
                break;
        }
    }
    
    public override void ReceiveMsg<T>(Connection observer, int MsgType, T value)
    {
        switch(MsgType)
        {
            //�o���u�̊J���擾
            case 0:
                type = GetValue<T, Valve_Base.Valve_Type>(value);
                Debug.Log("���ʊ���");
                break;
                
                //�o���u�̓���擾
            case 1:
                if (state == State.WaitStart)
                {
                    move_Valve = GetValue<T, bool>(value);
                    Debug.Log("����m�F");
                }
                break;
        }
    }  

    void move_Open()
    {
        //����
        change_localScale = transform.localScale; //���݂̃X�P�[�����i�[
        change_localScale.y += steam_Speed;  //�X�P�[���𑝂₷
        transform.localScale = change_localScale;  //�ύX�����X�P�[�����i�[
        now_Scale = change_localScale.y;  //���݂̃X�P�[�����L������
    }

    void move_Close()
    {
        change_localScale = transform.localScale;
        change_localScale.y -= steam_Speed;
        transform.localScale = change_localScale;
        now_Scale = change_localScale.y;
    }

    //����I��
  void stop_Open()
    {
        change_localScale = transform.localScale;
        change_localScale.y = now_Scale = steam_Scale;
        transform.localScale = change_localScale;
    }

    void stop_Close()
    {
        change_localScale = transform.localScale;
        change_localScale.y = now_Scale = 0;
        transform.localScale = change_localScale;
    }

}
