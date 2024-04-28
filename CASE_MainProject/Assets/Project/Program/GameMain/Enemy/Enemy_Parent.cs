using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Parent : MonoBehaviour
{
    [SerializeField, Header("最大HP")]
    protected float maxHp;
    [SerializeField, Header("現在のHP"), ReadOnly]
    protected float currentHp;
    [SerializeField, Header("最大圧力")]
    protected float maxPressure;
    [SerializeField, Header("現在の圧力"), ReadOnly]
    protected float currentPressure;


    protected void Start()
    {
        currentHp = maxHp;
        currentPressure = maxPressure;
        Debug.Log("開始");
    }

    // Update is called once per frame
    protected void Update()
    {
        Debug.Log("更新");
    }

    /*
     * <summary>
     * ダメージ関数
     * <param>
     * float : val ...ダメージ数
     * <retrun>
     * なし
     */
    public void Damage(float val)
    {
        currentHp -= val;
        DestroyCheck();
    }

    /*
     * <summary>
     * 死亡判定関数
     * <param>
     * なし
     * <return>
     * なし
     */
    protected void DestroyCheck()
    {
        if(currentHp <= 0.0f)
        {
            // 死亡アニメーション再生
            // 死亡後破壊
            Destroy(gameObject);
        }
    }
}
