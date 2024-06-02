using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Steam_Scale : Subscriber
{
    [SerializeField, Header("���ˑ��x")]
    float steam_Speed;

    [SerializeField, Header("�ő啬�ˋ���")]
    float steam_Scale;

    [SerializeField, Header("���ݕ��ˋ���"), ReadOnly]
    float now_Scale;

    [SerializeField, Header("���Ԍo�߂���"), ReadOnly]
    bool steam_Timer = false;

    [SerializeField, Header("���쎞��")]
    float setam_Timelimit;


    private bool move_Valve;  //�쓮�������H
    private Valve_Base.Valve_Type type;  //�o���u�̎��
    Vector3 localScale;
    

    void Start()
    {
        //�X�`�[�����ŏ�����o���Ă��邩�ۂ��H
      switch(type)
        {
            case Valve_Base.Valve_Type.open:
                now_Scale = 0;
                break;
            case Valve_Base.Valve_Type.close:
                now_Scale = steam_Scale;
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(move_Valve)
        {
            switch (type)
            {
                case Valve_Base.Valve_Type.open:
                    move_Open();
                    break;
                case Valve_Base.Valve_Type.close:
                    move_Close(); 
                    break;
            }
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
                move_Valve = GetValue<T, bool>(value);
                Debug.Log("����m�F");
                break;
        }
    }  

    void move_Open()
    {
        localScale = transform.localScale;
        localScale.y += steam_Speed;
        now_Scale = localScale.y;
        transform.localScale = localScale;
    }

    void move_Close()
    {
        localScale = transform.localScale;
        localScale.y -= steam_Speed;
        now_Scale = localScale.y;
        transform.localScale = localScale;
    }


}
