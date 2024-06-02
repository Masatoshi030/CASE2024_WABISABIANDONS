using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Steam_Scale : Subscriber
{
    [SerializeField, Header("噴射速度")]
    float steam_Speed;

    [SerializeField, Header("最大噴射距離")]
    float steam_Scale;

    [SerializeField, Header("現在噴射距離"), ReadOnly]
    float now_Scale;

    [SerializeField, Header("時間経過あり"), ReadOnly]
    bool steam_Timer = false;

    [SerializeField, Header("動作時間")]
    float setam_Timelimit;


    private bool move_Valve;  //作動したか？
    private Valve_Base.Valve_Type type;  //バルブの種類
    Vector3 localScale;
    

    void Start()
    {
        //スチームを最初から出しているか否か？
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
            //バルブの開閉を取得
            case 0:
                type = GetValue<T, Valve_Base.Valve_Type>(value);
                Debug.Log("識別完了");
                break;
                
                //バルブの動作取得
            case 1:
                move_Valve = GetValue<T, bool>(value);
                Debug.Log("動作確認");
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
