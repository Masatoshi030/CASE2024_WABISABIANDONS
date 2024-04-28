using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTypeB : Enemy_Mob
{
    // Start is called before the first frame update
    void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
    }

    protected override void AttackFunc1()
    {
        throw new System.NotImplementedException();
    }

    protected override void AttackFunc2()
    {
        throw new System.NotImplementedException();
    }

    protected override void DeathFunc()
    {
        throw new System.NotImplementedException();
    }

    protected override void DestroyFunc()
    {
        base.DestroyFunc();
    }

    protected override void EscapeFunc()
    {
        throw new System.NotImplementedException();
    }

    protected override void HealFunc()
    {
        throw new System.NotImplementedException();
    }

    protected override void IdleFunc()
    {
        state = State.Move;
        Debug.Log("‘Ò‹@");
    }

    protected override void MoveFunc()
    {
        Debug.Log("ˆÚ“®");
    }

    protected override void SpecialFuncA()
    {
        throw new System.NotImplementedException();
    }

    protected override void SpecialFuncB()
    {
        throw new System.NotImplementedException();
    }

    protected override void TrackingFunc()
    {
        throw new System.NotImplementedException();
    }

}


