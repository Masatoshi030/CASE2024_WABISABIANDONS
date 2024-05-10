using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy_Mob : Enemy_Parent
{
    [System.Serializable]
    public struct EnemyAgentParam
    {
        [SerializeField, Header("移動速度")]public float moveSpeed;
        [SerializeField, Header("加速度")] public float moveAcceleration;
        [SerializeField, Header("回転速度")] public float angularSpeed;
        [SerializeField, Header("停止距離")] public float stoppingDistance;
        [SerializeField, Header("消費圧力")] public float consumePressure;
    }

    public enum State
    {
        [InspectorName("待機")] Idle,
        [InspectorName("移動")] Move,
        [InspectorName("追尾")] Tracking,
        [InspectorName("逃走")] Escape,
        [InspectorName("被弾")] Damaged,
        [InspectorName("攻撃A")] AttackA,
        [InspectorName("攻撃B")] AttackB,
        [InspectorName("固有A")] UniqueA,
        [InspectorName("固有B")] UniqueB,
        [InspectorName("固有C")] UniqueC,
        [InspectorName("固有D")] UniqueD,
        [InspectorName("回復")] Heal,
        [InspectorName("死亡待ち")]DeathWait,
        [InspectorName("死亡")] Death,
    }

    [Space(padA), Header("--視認パラメータ--")]
    [SerializeField, Header("敵視認距離")]
    protected float viewingDistance = 20.0f;

    [SerializeField, Header("視野角")]
    protected float viewingAngle = 80.0f;

    [SerializeField, Header("視点位置")]
    protected Transform eyeTransform;

    [Space(padB), SerializeField, Header("状態"), Toolbar(typeof(State))]
    protected State state = State.Idle;
    
    protected void Start()
    {
        if(eyeTransform == null)
        {
            eyeTransform = transform.Find("EyeTransform");
        }
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
            case State.Damaged: DamagedFunc(); break;
            case State.AttackA: AttackFuncA(); break;
            case State.AttackB: AttackFuncB(); break;
            case State.UniqueA: UniqueFuncA(); break;
            case State.UniqueB: UniqueFuncB(); break;
            case State.UniqueC: UniqueFuncC(); break;
            case State.UniqueD: UniqueFuncD(); break;
            case State.Heal: HealFunc(); break;
            case State.DeathWait: DeathWaitFunc(); break;
            case State.Death: DeathFunc(); break;
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
    protected virtual void IdleFunc() { }

    /*
     * <summary>
     * 移動状態関数
     * <param>
     * なし
     * <return>
     * なし
     */
    protected virtual void MoveFunc() { }

    /*
     * <summary>
     * 追尾状態関数
     * <param>
     * なし
     * <return>
     * なし
     */
    protected virtual void TrackingFunc() { }

    /*
     * <summary>
     * 逃走状態関数
     * <param>
     * なし
     * <return>
     * なし
     */
    protected virtual void EscapeFunc() { }

    /*
     * <summary>
     * 被弾状態関数
     * <param>
     * void
     * <return>
     * void
     */
    protected virtual void DamagedFunc() { }

    /*
     * <summary>
     * 攻撃関数パターンA
     * <param>
     * なし
     * <return>
     * なし
     */
    protected virtual void AttackFuncA() { }

    /*
     * <summary>
     * 攻撃関数パターンB
     * <param>
     * なし
     * <return>
     * なし
     */
    protected virtual void AttackFuncB() { }

    /*
     * <summary>
     * 固有関数パターンA
     * <param>
     * なし
     * <return>
     * なし
     */
    protected virtual void UniqueFuncA() { }

    /*
     * <summary>
     * 固有関数パターンB
     * <param>
     * なし
     * <return>
     * なし
     */
    protected virtual void UniqueFuncB() { }

    /*
     * <summary>
     * 固有関数パターンC
     * <param>
     * なし
     * <return>
     * なし
     */
    protected virtual void UniqueFuncC() { }

    /*
     * <summary>
     * 固有関数パターンD
     * <param>
     * なし
     * <return>
     * なし
     */
    protected virtual void UniqueFuncD() { }

    /*
     * <summary>
     * 回復状態関数
     * <param>
     * なし
     * <return>
     * なし
     */
    protected virtual void HealFunc() { }

    /*
     * <summary>
     * 死亡待ち関数
     * <param>
     * void
     * <return>
     * void
     */
    protected virtual void DeathWaitFunc() { }

    /*
     * <summary>
     * 死亡状態関数
     * <param>
     * なし
     * <return>
     * なし
     */
    protected virtual void DeathFunc() { }


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
        Vector3 Diff = target.transform.position - eyeTransform.position;
        float distance = Diff.x * Diff.x + Diff.y * Diff.y + Diff.z * Diff.z;

        if (distance < viewingDistance * viewingDistance)
        {
            // 外積でy軸から正面の左右どちらにあるか求める
            Vector3 axis = Vector3.Cross(eyeTransform.forward, Diff);

            // 角度が+なら正面より右にいる-なら左にいる
            float angle = Vector3.Angle(eyeTransform.forward, Diff) * (axis.y < 0 ? -1.0f : 1.0f);

            if (Mathf.Abs(angle) < viewingAngle / 2)
            {
                Vector3 Direction = Diff.normalized;
                // レイの作成と表示
                Ray ray = new Ray(eyeTransform.position, Direction);
                Debug.DrawRay(eyeTransform.position, Direction, Color.red);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, viewingDistance))
                {
                    if (hit.transform.root.name == "Player")
                    {
                        return (true, distance);
                    }
                }
            }
        }
        return (false, distance);
    }

    /*
    * <summary>
    * オブジェクト探索関数
    * <param>
    * GameObject[] objects
    * <return>
    * bool isFind, float distance, GameObject findObject
    */
    protected (bool isFind, float distance, GameObject findObject) FindObjectAtFOV(GameObject[] objects)
    {
        // 距離を測る
        Vector3 Diff = target.transform.position - eyeTransform.position;
        float distance = Diff.x * Diff.x + Diff.y * Diff.y + Diff.z * Diff.z;

        if (distance < viewingDistance)
        {
            // 外積でy軸から正面の左右どちらにあるか求める
            Vector3 axis = Vector3.Cross(eyeTransform.forward, Diff);

            // 角度が+なら正面より右にいる-なら左にいる
            float angle = Vector3.Angle(eyeTransform.forward, Diff) * (axis.y < 0 ? -1.0f : 1.0f);

            if (Mathf.Abs(angle) < viewingAngle / 2)
            {
                Vector3 Direction = Diff.normalized;
                // レイの作成と表示
                Ray ray = new Ray(eyeTransform.position, Direction);
                Debug.DrawRay(eyeTransform.position, Direction, Color.red);
                RaycastHit[] hits = Physics.RaycastAll(eyeTransform.position, Direction, viewingDistance);
                if (hits.Length > 0)
                {
                    for(int i = 0; i < objects.Length; i++)
                    {
                        if (hits[0].transform.gameObject == objects[i])
                        {
                            return (true, distance, objects[i]);
                        }
                    }
                }
            }
        }
        return (false, distance, null);
    }

    /*
    * <summary>
    * オブジェクト探索関数
    * <param>
    * string[] tags
    * <return>
    * bool isFind, float distance, string findTag
    */
    protected (bool isFind, float distance, string findTag) FindObjectAtFOV(string[] tags)
    {
        // 距離を測る
        Vector3 Diff = target.transform.position - eyeTransform.position;
        float distance = Diff.x * Diff.x + Diff.y * Diff.y + Diff.z * Diff.z;

        if (distance < viewingDistance)
        {
            // 外積でy軸から正面の左右どちらにあるか求める
            Vector3 axis = Vector3.Cross(eyeTransform.forward, Diff);

            // 角度が+なら正面より右にいる-なら左にいる
            float angle = Vector3.Angle(eyeTransform.forward, Diff) * (axis.y < 0 ? -1.0f : 1.0f);

            if (Mathf.Abs(angle) < viewingAngle / 2)
            {
                Vector3 Direction = Diff.normalized;
                // レイの作成と表示
                Ray ray = new Ray(eyeTransform.position, Direction);
                Debug.DrawRay(eyeTransform.position, Direction, Color.red);
                RaycastHit[] hits = Physics.RaycastAll(eyeTransform.position, Direction, viewingDistance);
                if (hits.Length > 0)
                {
                    for (int i = 0; i < tags.Length; i++)
                    {
                        if (hits[0].transform.tag == tags[i])
                        {
                            return (true, distance, tags[i]);
                        }
                    }
                }
            }
        }
        return (false, distance, null);
    }

    /*
    * <summary>
    * 状態判断関数(仮想)
    * <param>
    * void
    * <return>
    * void
    */
    protected virtual void JudgeState()
    {
        // ステートのジャッジ(アニメーションの終了時に叩いたり)
    }

    /*
    * <summary>
    * 状態設定関数
    * <param>
    * Enemy_Mob.State state
    * <return>
    * void
    */
    public void SetState(State state)
    {
        this.state = state;
    }
}
