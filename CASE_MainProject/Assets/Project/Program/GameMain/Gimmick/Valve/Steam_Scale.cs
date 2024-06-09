using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class Steam_Scale : Subscriber
{
    public enum State
    {
        WaitStart, RunningToEnd, End, RunningToStart,
    }

    [SerializeField, Header("状態"), Toolbar(typeof(State))]
    public State state = State.WaitStart;

    [SerializeField, Header("噴射速度")]
    float steam_Speed;

    [SerializeField, Header("最大噴射距離")]
    float steam_Scale;

    [SerializeField, Header("現在噴射距離"), ReadOnly]
    float now_Scale;

    [SerializeField, Header("時間経過あり")]
    bool steam_Timer = true;

    [SerializeField, Header("動作時間")]
    float steam_Limit;

    [SerializeField, Header("バルブの開閉アニメーション")]
    Animator valveOpenCloseAnimator;

    [SerializeField, Header("蒸気エフェクト")]
    ParticleSystem steamEffect;

    private float nothing_Steam = 0;  //スチームが出てない状態

    private bool move_Valve;  //作動したか？
    private Valve_Base.Valve_Type type;  //バルブの種類
    Vector3 change_localScale;  //変更するスケールを一時補完する
    private float limit;  //制限時間を測る

    void Start()
    {
        switch (type)
        {
            case Valve_Base.Valve_Type.open:
                change_localScale = transform.localScale;
                change_localScale.y = nothing_Steam;
                Debug.Log("0に設定");

                valveOpenCloseAnimator.SetBool("bOpen", false);
                SetActiveSteam(1.0f, false);

                break;

            case Valve_Base.Valve_Type.close:
                change_localScale = transform.localScale;
                change_localScale.y = steam_Scale; //スチームが出ている状態
                Debug.Log("最大値の設定");

                valveOpenCloseAnimator.SetBool("bOpen", true);
                SetActiveSteam(1.0f, true);
                break;
        }

        //大きさの変更を格納
        this.transform.localScale = change_localScale;
    }

    // Update is called once per frame
    void Update()
    {
        //状態
        switch (state)
        {
            //発動前
            case State.WaitStart: 
                if (move_Valve)
                { state = State.RunningToEnd; }  //発動
                move_Valve = false;
                break;

            //発動中
            case State.RunningToEnd:
                switch (type)
                    {
                        case Valve_Base.Valve_Type.open: //バルブを開ける
                        move_Open();
                        if(now_Scale>=steam_Scale)
                        {
                            stop_Open();
                            state = State.End;
                        }
                        break;

                        //バルブを閉める
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
            //発動前に戻る
            case State.RunningToStart:
            
                switch (type)
                {
                    //バルブを開ける
                    case Valve_Base.Valve_Type.open:
                        move_Close();
                        if (now_Scale <= nothing_Steam)
                        {
                            stop_Close();
                            state = State.WaitStart;
                        }
                        break;

                    //バルブを閉める
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

            //発動後
            case State.End:                
               
                if(steam_Timer)
                {
                    limit += Time.deltaTime;

                    if (limit > steam_Limit)
                    {
                        state = State.RunningToStart;
                        limit = 0;

                        if (type == Valve_Base.Valve_Type.open)
                        {
                            valveOpenCloseAnimator.SetBool("bOpen", false);
                            SetActiveSteam(1.0f, false);
                        }
                        else if (type == Valve_Base.Valve_Type.close)
                        {
                            valveOpenCloseAnimator.SetBool("bOpen", true);
                            SetActiveSteam(1.0f, true);
                        }
                    }
                }
                break;
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
                if (state == State.WaitStart)
                {
                    move_Valve = GetValue<T, bool>(value);
                    Debug.Log("動作確認");

                    if(type == Valve_Base.Valve_Type.open)
                    {
                        valveOpenCloseAnimator.SetBool("bOpen", true);
                        SetActiveSteam(1.0f, true);
                    }
                    else if(type == Valve_Base.Valve_Type.close)
                    {
                        valveOpenCloseAnimator.SetBool("bOpen", false);
                        SetActiveSteam(1.0f, false);
                    }
                }
                break;
        }
    }  

    void move_Open()
    {
        //動作
        change_localScale = transform.localScale; //現在のスケールを格納
        change_localScale.y += steam_Speed;  //スケールを増やす
        transform.localScale = change_localScale;  //変更したスケールを格納
        now_Scale = change_localScale.y;  //現在のスケールを記憶する
    }

    void move_Close()
    {
        change_localScale = transform.localScale;
        change_localScale.y -= steam_Speed;
        transform.localScale = change_localScale;
        now_Scale = change_localScale.y;
    }

    //動作終了
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

    public async void SetActiveSteam(float _time, bool _active)
    {
        // ヒットストップの長さだけ待機
        await Task.Delay((int)(_time * 1000));

        //スチームをセット
        if (_active)
        {
            steamEffect.Play();
        }
        else
        {
            steamEffect.Stop();
        }
    }
}
