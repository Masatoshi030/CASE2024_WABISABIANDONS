using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy_Mob : Enemy_Parent
{
    public enum State
    {
        [InspectorName("待機")] Idle,
        [InspectorName("移動")] Move,
        [InspectorName("追尾")] Tracking,
        [InspectorName("逃走")] Escape,
        [InspectorName("攻撃A")] AttackA,
        [InspectorName("攻撃B")] AttackB,
        [InspectorName("特殊A")]SpecialA,
        [InspectorName("特殊B")]SpecialB,
        [InspectorName("回復")] Heal,
        [InspectorName("破壊")] Death,
    }

    [SerializeField, Header("敵視認距離")]
    protected float viewingDistance = 20.0f;

    [SerializeField, Header("視野角")]
    protected float viewingAngle = 80.0f;

    [SerializeField, Header("視点の高さ")]
    protected float eyeHeight = 1.0f;

    [SerializeField, Header("状態"), Toolbar(typeof(State))]
    protected State state = State.Idle;
    
    protected void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected void Update()
    {
        base.Update();
        switch (state)
        {
            case State.Idle:IdleFunc(); break;
            case State.Move: MoveFunc(); break;
            case State.Tracking: TrackingFunc(); break;
            case State.Escape: EscapeFunc(); break;
            case State.AttackA: AttackFunc1(); break;
            case State.AttackB: AttackFunc2(); break;
            case State.SpecialA: SpecialFuncA(); break;
            case State.SpecialB: SpecialFuncB(); break;
            case State.Heal: HealFunc(); break;
            case State.Death: break;
        }
    }

    /*
     * <summary>
     * 待機状態関数
     * <param>
     * なし
     * <return>
     * なし
     */
    protected abstract void IdleFunc();

    /*
     * <summary>
     * 移動状態関数
     * <param>
     * なし
     * <return>
     * なし
     */
    protected abstract void MoveFunc();

    /*
     * <summary>
     * 追尾状態関数
     * <param>
     * なし
     * <return>
     * なし
     */
    protected abstract void TrackingFunc();

    /*
     * <summary>
     * 逃走状態関数
     * <param>
     * なし
     * <return>
     * なし
     */
    protected abstract void EscapeFunc();

    /*
     * <summary>
     * 攻撃関数パターンA
     * <param>
     * なし
     * <return>
     * なし
     */
    protected abstract void AttackFunc1();

    /*
     * <summary>
     * 攻撃関数パターンB
     * <param>
     * なし
     * <return>
     * なし
     */
    protected abstract void AttackFunc2();

    /*
     * <summary>
     * 攻撃関数パターンB
     * <param>
     * なし
     * <return>
     * なし
     */
    protected abstract void SpecialFuncA();

    /*
     * <summary>
     * 攻撃関数パターンB
     * <param>
     * なし
     * <return>
     * なし
     */
    protected abstract void SpecialFuncB();

    /*
     * <summary>
     * 回復状態関数
     * <param>
     * なし
     * <return>
     * なし
     */
    protected abstract void HealFunc();

    /*
     * <summary>
     * 死亡状態関数
     * <param>
     * なし
     * <return>
     * なし
     */
    protected abstract void DeathFunc();

    protected override void DestroyFunc()
    {

    }

    /*
     * <summary>
     * プレイヤーを視野角を元に探す処理
     * <param>
     * なし
     * <returns>
     * bool isFind, float distance
     */
    protected (bool isFind, float distance) FindPlayerAtFOV()
    {
        // 距離を測る
        float distance = Vector3.Distance(target.transform.position, transform.position);

        if (distance < viewingDistance)
        {
            // 自分からターゲットに向かうベクトル
            Vector3 MetoTarget = target.transform.position - transform.position;
            // ターゲットから自分に向かうベクトル
            Vector3 TargettoMe = -MetoTarget;
            // 外積でy軸から正面の左右どちらにあるか求める
            Vector3 axis = Vector3.Cross(transform.forward, MetoTarget);

            // 角度が+なら正面より右にいる-なら左にいる
            float angle = Vector3.Angle(transform.forward, MetoTarget) * (axis.y < 0 ? -1.0f : 1.0f);

            if (Mathf.Abs(angle) < viewingAngle / 2)
            {
                Vector3 Direction = MetoTarget.normalized;
                // 目の位置を決める
                Vector3 eyePos = transform.position + new Vector3(0.0f, eyeHeight, 0.0f);
                // 例の作成と表示
                Ray ray = new Ray(eyePos, Direction);
                Debug.DrawRay(eyePos, Direction, Color.red);
                RaycastHit[] hits = Physics.RaycastAll(eyePos, Direction, viewingDistance);
                if (hits.Length > 0)
                {
                    if (hits[0].transform.tag == "Player")
                    {
                        return (true, distance);
                    }
                }
            }
        }
        return (false, distance);
    }
}
