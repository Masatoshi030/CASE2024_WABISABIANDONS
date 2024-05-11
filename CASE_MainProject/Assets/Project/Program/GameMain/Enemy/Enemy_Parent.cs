using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;

public abstract class Enemy_Parent : MonoBehaviour
{
    protected const float padA = 20.0f;
    protected const float padB = 10.0f;
    public enum EnemyType
    { 
        [InspectorName("モブ")]Mob, 
        [InspectorName("ボス")]Boss
    };

    public struct EnemySetting
    {
        public EnemyType type;
        public float hp;
        public float pressure;
    }

    [SerializeField, Header("敵タイプ"), Toolbar(typeof(EnemyType))]
    protected EnemyType enemyType;

    [Space(padA), Header("--基礎パラメータ--")]
    [SerializeField, Header("最大HP")]
    protected float maxHp;
    [SerializeField, Header("現在のHP"), ReadOnly]
    protected float currentHp;
    [SerializeField, Header("最大圧力")]
    protected float maxPressure;
    [SerializeField, Header("現在の圧力"), ReadOnly]
    protected float currentPressure;

    protected static GameObject target;

    protected void Awake()
    {
        if (target == null)
        {
            target = GameObject.Find("Player");
            if (target != null)
            {
                Debug.Log("Playerの格納完了");
            }
        }
    }

    protected void Start()
    {
        currentHp = maxHp;
        currentPressure = maxPressure;
    }

    // Update is called once per frame
    protected void Update()
    {

    }

    /*
     * <summary>
     * ダメージ関数
     * <param>
     * float : val ...ダメージ数, Vector3 direction...向き
     * <retrun>
     * なし
     */
    public virtual bool Damage(float val, Vector3 direction)
    {
        currentHp -= val;
        return DestroyCheck();
    }

    /*
     * <summary>
     * 死亡判定関数
     * <param>
     * なし
     * <return>
     * なし
     */
    protected bool DestroyCheck()
    {
        if(currentHp <= 0.0f)
        {
            // 子クラスの破壊関数を呼び出す
            DestroyFunc();
            return true;
        }
        return false;
    }

    /*
     * <summary>
     * 死亡時関数
     * <param>
     * なし
     * <return>
     * なし
     */
    protected virtual void DestroyFunc() { }

    /*
     * <summary>
     * 圧力の加算関数
     * <param>
     * float pressure
     * <return>
     * void
     */
    public virtual void AddPressure(float pressure)
    {
        currentPressure += pressure;
        if (currentPressure > maxPressure) currentPressure = maxPressure;
    }

    /*
     * <summary>
     * 圧力の減算関数
     * <param>
     * float pressure
     * <return>
     * void
     */
    public virtual void SubPressure(float pressure)
    {
        currentPressure -= pressure;
        if(currentPressure < 0.0f) currentPressure = 0.0f;
    }

    /*
     * <summary>
     * 圧力の取得関数
     * <param>
     * void
     * <return>
     * float
     */
    public float GetPressure() { return currentPressure; }
}
