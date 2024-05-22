using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Subscriber_MoveFloor : Subscriber
{
    enum State
    {
        WaitStart, RunningToEnd, End, RunningToStart,
    }
    enum Direction
    {
        Forward, Back, Right, Left, Up, Down
    }

    [SerializeField, Header("移動軸"), Toolbar(typeof(Direction))]
    Direction axis = Direction.Forward;
    [SerializeField, Header("状態"), Toolbar(typeof(State))]
    State state = State.WaitStart;
    [SerializeField, Header("移動速度")]
    float moveMagnification = 1.0f;
    [SerializeField, Header("現在の移動量"), ReadOnly]
    float moveValue = 0.0f;
    [SerializeField, Header("最大移動量")]
    float maxMoveValue = 1.0f;

    Vector3 initPosition;

    private void Start()
    {
        initPosition = transform.position;
    }

    private void Update()
    {
        Vector3 position = transform.position;
        Vector3 velocity = Vector3.zero;
        switch (axis)
        {
            case Direction.Forward: velocity = transform.forward * moveValue;position =  initPosition + velocity; break;
            case Direction.Back: velocity = -transform.forward * moveValue; position = initPosition + velocity; break;
            case Direction.Right: velocity = transform.right * moveValue; position = initPosition + velocity; break;
            case Direction.Left: velocity = -transform.right * moveValue; position = initPosition + velocity; break;
            case Direction.Up: velocity = transform.up * moveValue; position = initPosition + velocity; break;
            case Direction.Down: velocity = -transform.up * moveValue; position = initPosition + velocity; break;
        }
        transform.position = position;
    }

    public override void ReceiveMsg<T>(Publisher observer,int MsgType, T value)
    {
        switch (MsgType)
        {
            case 0:
                // 経過時間の通知
                float val = GetValue<T, float>(value);
                moveValue = val * moveMagnification > maxMoveValue ? maxMoveValue : val * moveMagnification;
                break;
            case 1:
                bool msg = GetValue<T, bool>(value);
                break;
        }
    }
}
