using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObj : Subscriber
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
    Vector3 velocity = Vector3.zero;
    bool On = false;

    void Start()
    {
        initPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 position = transform.position;


        if (On)
        {
            moveValue = moveMagnification;
        }

        switch (axis)
        {
            case Direction.Forward: velocity = transform.forward * moveValue; position = position + velocity; break;
            case Direction.Back: velocity = -transform.forward * moveValue; position = position + velocity; break;
            case Direction.Right: velocity = transform.right * moveValue; position = position + velocity; break;
            case Direction.Left: velocity = -transform.right * moveValue; position = position + velocity; break;
            case Direction.Up: velocity = transform.up * moveValue; position = position + velocity; break;
            case Direction.Down: velocity = -transform.up * moveValue; position = position + velocity; break;
        }
        transform.position = position;
    }

    public override void ReceiveMsg<T>(Connection observer, int MsgType, T value)
    { 
    switch(MsgType)
        {
            case 0:
                On = GetValue<T, bool>(value);

                break;
        }
        Debug.Log("bool作動");
    }
}
