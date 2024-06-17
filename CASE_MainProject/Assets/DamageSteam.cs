using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageSteam : MonoBehaviour
{

    [SerializeField, Header("蒸気エフェクト")]
    ParticleSystem steamEffect;

    [SerializeField, Header("当たり判定")]
    Collider steamCollider;

    [SerializeField, Header("噴出中")]
    bool bSteam = true;

    [SerializeField, Header("インターバル有効")]
    bool bIntervalDisable = true;

    [SerializeField, Header("インターバル時間")]
    float intervalTime = 3.0f;

    [SerializeField, Header("噴出時間")]
    float steamTime = 2.0f;

    [SerializeField, Header("タイマー")]
    float timer = 0.0f;

    [SerializeField, Header("バルブの振動")]
    SinVibration mySinVibration;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //インターバル機能が有効だったら
        if (bIntervalDisable)
        {
            //タイマーを進める
            timer += Time.deltaTime;

            if (bSteam)
            {
                if(timer > steamTime)
                {
                    //噴出停止状態
                    bSteam = false;

                    //エフェクトを止める
                    steamEffect.Stop();

                    //当たり判定を無効にする
                    steamCollider.enabled = false;

                    //振動開始
                    mySinVibration.enabled = true;

                    //タイマーをリセット
                    timer = 0.0f;
                }
            }
            else
            {
                if (timer > intervalTime)
                {
                    //噴出状態
                    bSteam = true;

                    //エフェクトを再生する
                    steamEffect.Play();

                    //当たり判定を有効にする
                    steamCollider.enabled = true;

                    //振動終了
                    mySinVibration.enabled = false;

                    timer = 0.0f;
                }
            }
        }
        else
        {

        }
    }
}
