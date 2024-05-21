using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GimmickState : State
{
    protected Gimmick gimmick;

    public Gimmick GimmickObject { get => gimmick; set => gimmick = value; }

    [SerializeField, Header("アニメーション速度")]
    protected float animSpeed = 1.0f;
    [SerializeField, Header("開始時アニメーション")]
    protected string enterAnimaion;

    public override void Enter()
    {
        gimmick.GimmickAnimator.speed = animSpeed;
        gimmick.GimmickAnimator.Play(enterAnimaion);
    }
}
