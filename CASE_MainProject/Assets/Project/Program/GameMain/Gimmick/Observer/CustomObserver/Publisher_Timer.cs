using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Publisher_Timer : Publisher
{
    enum State
    { Running, Interval};
    [SerializeField, Header("���"), Toolbar(typeof(State))]
    State state = State.Interval;
    [SerializeField, Header("�ő�J�E���g����")]
    float maxCountTime = 1.0f;
    public float MaxCount { get => maxCountTime; }
    [SerializeField, Header("�o�ߎ���"), ReadOnly]
    float elapsedTime = 0.0f;
    [SerializeField, Header("�C���^�[�o������")]
    float intervalTime;
    [SerializeField, Header("�C���^�[�o���o�ߎ���"), ReadOnly]
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
            // �ړ��̏I���ʒm
            SendMsg<bool>(1, true);
        }
        else if (elapsedTime < 0.0f)
        {
            isAdd = true;
            state = State.Interval;
            elapsedTime = 0.0f;
            // �ړ��̏I���ʒm
            SendMsg<bool>(1, false);
        }

        // �o�ߎ��Ԃ�ʒm
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
