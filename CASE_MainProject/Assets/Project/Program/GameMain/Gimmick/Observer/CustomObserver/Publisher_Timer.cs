using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Publisher_Timer : Publisher
{
    enum State
    { Running, Interval};
    [SerializeField, Header("状態"), Toolbar(typeof(State))]
    State state = State.Interval;
    [SerializeField, Header("最大カウント時間")]
    float maxCountTime = 1.0f;
    public float MaxCount { get => maxCountTime; }
    [SerializeField, Header("経過時間"), ReadOnly]
    float elapsedTime = 0.0f;
    [SerializeField, Header("インターバル時間")]
    float intervalTime;
    [SerializeField, Header("インターバル経過時間"), ReadOnly]
    float intervalCnt = 0.0f;

    bool isAdd = false;

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case State.Interval: IntervalFunc(); break;
            case State.Running: RunningFunc(); break;
        }
    }

    void RunningFunc()
    {
        if (isAdd)
            elapsedTime += Time.deltaTime;
        else
            elapsedTime -= Time.deltaTime;
        if (elapsedTime >= maxCountTime)
        {
            isAdd = false;
            state = State.Interval;
            elapsedTime = maxCountTime;
            // 移動の終了通知
            SendMsg<bool>(1, true);
        }
        else if (elapsedTime < 0.0f)
        {
            isAdd = true;
            state = State.Interval;
            elapsedTime = 0.0f;
            // 移動の終了通知
            SendMsg<bool>(1, false);
        }

        // 経過時間を通知
        SendMsg<float>(0, elapsedTime);
    }
    void IntervalFunc()
    {
        intervalCnt += Time.deltaTime;
        if (intervalCnt >= intervalTime)
        {
            intervalCnt = 0.0f;
            state = State.Running;
        }
    }
}
