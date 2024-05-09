using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class HitStopManager : MonoBehaviour
{
    //シングルトンインスタンス
    public static HitStopManager instance; // インスタンスの定義

    private void Awake()
    {
        if (instance == null)
        {
            // 自身をインスタンスとする
            instance = this;
        }
        else
        {
            // インスタンスが既に存在していたら自身を消去する
            Destroy(gameObject);
        }
    }

    public async void HitStopEffect(float _timeScale, float _stopTime)
    {
        // ゲームを停止
        Time.timeScale = _timeScale;

        // ヒットストップの長さだけ待機
        await Task.Delay((int)(_stopTime * 1000));

        // 待機が終わったらゲームを再開
        Time.timeScale = 1f;
    }
}
