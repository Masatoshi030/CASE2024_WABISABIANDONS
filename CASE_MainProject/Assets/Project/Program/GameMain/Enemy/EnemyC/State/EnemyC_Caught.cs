using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyC_Caught : EnemyState
{
    [SerializeField, Header("巻き込み中か"), ReadOnly]
    bool bCaught = false;
    public bool Caught { get => bCaught; set => bCaught = value; }

    [Space(pad), Header("--遷移先リスト--")]
    [SerializeField, Header("状態終了時の遷移")]
    int elapesedID;
    [SerializeField, Header("被弾時の遷移")]
    int damagedID;

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

        if(!bCaught)
        {
            machine.TransitionTo(elapesedID);
        }
    }

    public override void Exit()
    {
        bCaught = false;
    }
}
