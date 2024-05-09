using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 敵テンプレート

public class Enemy_Templete : Enemy_Mob
{
    // インスペクターが見やすいように関連ごとに分けるのもあり
    [Space(padA), Header("--メインパラメータ--")]
    [SerializeField, Header("パラメータ(仮)")]
    float param;

    [Space(padA), Header("--インターバル--")]
    [SerializeField, Header("インターバル(仮)")]
    float interval;

    [Space(padA), Header("--攻撃関連--")]
    [SerializeField, Header("攻撃(仮)")]
    float attackPower;

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

    /*
    * <summary>
    * 待機状態関数
    * <param>
    * void
    * <return>
    * void
    */
    protected override void IdleFunc()
    {
        throw new System.NotImplementedException();
    }

    /*
    * <summary>
    * 移動状態関数
    * <param>
    * void
    * <return>
    * void
    */
    protected override void MoveFunc()
    {
        throw new System.NotImplementedException();
    }

    /*
    * <summary>
    * 追尾状態関数
    * <param>
    * void
    * <return>
    * void
    */
    protected override void TrackingFunc()
    {
        throw new System.NotImplementedException();
    }

    /*
    * <summary>
    * 逃亡状態関数
    * <param>
    * void
    * <return>
    * void
    */
    protected override void EscapeFunc()
    {
        throw new System.NotImplementedException();
    }

    /*
    * <summary>
    * 攻撃関数A
    * <param>
    * void
    * <return>
    * void
    */
    protected override void AttackFuncA()
    {
        throw new System.NotImplementedException();
    }

    /*
    * <summary>
    * 攻撃関数B
    * <param>
    * void
    * <return>
    * void
    */
    protected override void AttackFuncB()
    {
        throw new System.NotImplementedException();
    }

    /*
    * <summary>
    * 特殊関数A
    * <param>
    * void
    * <return>
    * void
    */
    protected override void SpecialFuncA()
    {
        throw new System.NotImplementedException();
    }

    /*
    * <summary>
    * 特殊関数B
    * <param>
    * void
    * <return>
    * void
    */
    protected override void SpecialFuncB()
    {
        throw new System.NotImplementedException();
    }

    /*
    * <summary>
    * 回復関数
    * <param>
    * void
    * <return>
    * void
    */
    protected override void HealFunc()
    {
        throw new System.NotImplementedException();
    }

    /*
    * <summary>
    * 死亡状態関数
    * <param>
    * void
    * <return>
    * void
    */
    protected override void DeathFunc()
    {
        throw new System.NotImplementedException();
    }

    /*
    * <summary>
    * 破壊時呼び出し関数
    * <param>
    * void
    * <return>
    * void
    */
    protected override void DestroyFunc()
    {
        base.DestroyFunc();
    }
}
