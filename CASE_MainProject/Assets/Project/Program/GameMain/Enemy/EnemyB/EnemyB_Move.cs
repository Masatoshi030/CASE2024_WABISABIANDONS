using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ã‰º‚ÉˆÚ“®‚·‚é
public class EnemyB_Move : EnemyState
{
    [SerializeField, Header("‰ŠúˆÊ’u"), ReadOnly]
    Vector3 initPos;
    [SerializeField, Header("ˆÚ“®ŽžŠÔ")]
    float moveTime;
    float moveAmount;
    [SerializeField, Header("ˆÚ“®‘¬“x")]
    float moveSpeed;
    [SerializeField, Header("‰ŠúˆÚ“®•ûŒü‚ªã")]
    bool initIsUp = true;
    bool nowIsUp;

    [Space(pad), Header("‘JˆÚæƒŠƒXƒg")]
    [SerializeField, Header("õ“G¬Œ÷Žž‚Ì‘JˆÚ")]
    public int searchSuccessID;
    [SerializeField, Header("ŽžŠÔŒo‰ßŒã‚Ì‘JˆÚ")]
    public int elapsedID;
    [SerializeField, Header("”í’eŽž‚Ì‘JˆÚ")]
    public int damagedID;

    public override void Initialize()
    {
        initPos = enemy.transform.position;
        nowIsUp = initIsUp;
    }

    public override void Enter()
    {

    }

    public override void MainFunc()
    {
        if(enemy.IsDamaged)
        {
            machine.TransitionTo(damagedID);
            return;
        }

        // 1ƒtƒŒ[ƒ€“–‚½‚è‚ÌˆÚ“®—Ê
        float moveValue = moveSpeed * Time.deltaTime;
        if(machine.Cnt >= moveTime)
        {
            // ’´‰ß—Ê‚ð‚à‚Ç‚·
            float gap = machine.Cnt - moveTime;
            moveValue -= gap * moveSpeed;
        }


        if (nowIsUp)
        {
            enemy.transform.Translate(enemy.transform.up * moveValue);
        }
        else
        {
            enemy.transform.Translate(-enemy.transform.up * moveValue);
        }

        if(machine.Cnt >= moveTime)
        {
            nowIsUp = !nowIsUp;
            machine.TransitionTo(elapsedID);
        }

        if(enemy.IsFindPlayer)
        {
            machine.TransitionTo(searchSuccessID);
        }
    }
}
