using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

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

    [SerializeField,Header("バルブの色"),ReadOnly]
    Valve_Base.Valve_Type color;
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
    public bool On = false;

    
    void Start()
    {
        initPosition = transform.position;  //最初の場所の格納
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 position = transform.position;  //現在ポジション


        //状態の遷移
        switch (state)
        {
            case State.WaitStart:
                if (On) { state = State.RunningToEnd; }
                break;

            case State.RunningToEnd:
              position=moving(position);
                On = false;  //状態を戻す
                if (moveValue >= maxMoveValue)
                { state = State.End; }
                break;

            case State.RunningToStart:
                moveMagnification = -moveMagnification;
                break;

            case State.End:
               
                break;
        }



        transform.position = position;
    }

    //移動関数
    public Vector3 moving(Vector3 pos)
    {
        switch (axis)
        {
            case Direction.Forward: velocity = transform.forward * moveMagnification; pos = pos + velocity; break;
            case Direction.Back: velocity = -transform.forward * moveMagnification; pos = pos + velocity; break;
            case Direction.Right: velocity = transform.right * moveMagnification; pos = pos + velocity; break;
            case Direction.Left: velocity = -transform.right * moveMagnification; pos = pos + velocity; break;
            case Direction.Up: velocity = transform.up * moveMagnification;     pos = pos + velocity; break;
            case Direction.Down: velocity = -transform.up * moveMagnification; pos = pos + velocity; break;
        }
        moveValue += moveMagnification;  //移動量を加算していく
        return pos;
    }

    public override void ReceiveMsg<T>(Connection observer, int MsgType, T value)
    { 
    switch(MsgType)
        {
            case 0:
                break;

            case 1:
                On = GetValue<T, bool>(value);
                Debug.Log("bool作動");
                break;
        }

    }
}
