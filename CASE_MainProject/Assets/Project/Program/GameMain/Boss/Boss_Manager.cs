using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Manager : MonoBehaviour
{
    public enum Emotion
    {
        [InspectorName("冷静")]Calm,      // 遠距離偏り
        [InspectorName("怒り")] Anger,    // 物理偏り
        [InspectorName("暴走")]Lazy,      // 特殊
        Max
    }

    [SerializeField, Header("感情配列")]
    int[] emotionArray;
    [SerializeField,Header("感情配列(float)")]
    float[] fEmotionArray;
    [SerializeField, Header("感情"), Toolbar(typeof(Emotion))]
    Emotion emotion;
    // 感情の傾き。感情決定時の重き
    Emotion emotionTilt;

    // Start is called before the first frame update
    void Start()
    {
        emotionArray = new int[(int)Emotion.Max];
        fEmotionArray = new float[(int)Emotion.Max];
        emotionArray[(int)Emotion.Calm] = 10;
    }

    // Update is called once per frame
    void Update()
    {
        // 感情の計算
        float total = 0;

        for(int i = 0; i < (int)Emotion.Max; i++)
        {
            total += emotionArray[i];
        }

        // パーセンテージに変換(100%)
        for(int i = 0; i < (int)Emotion.Max; i++)
        {
            fEmotionArray[i] = ((float)emotionArray[i] / (float)total) * 100.0f;
        }
    }

    // 感情を動かす
    public void TouchEmotion(Emotion emotion, int value)
    {
        // 特定の感情の計算
        emotionArray[(int)emotion] += value;
        if(emotionArray[(int)emotion] <= 0)
        {
            emotionArray[(int)emotion] = 0;
        }
    }

    public float[] GetEmotions()
    {
        return fEmotionArray;
    }

    // 感情の決定と取得
    public Emotion CalcEmotion()
    {
        float[] rates = fEmotionArray;
        // 傾きにボーナスを加える
        rates[(int)emotionTilt] += 50.0f;
        int r = Random.Range(0, 150);
        if(r <= rates[(int)Emotion.Calm])
        {
            // 冷静
            emotion = Emotion.Calm;
        }
        else if(r <= rates[(int)Emotion.Calm] + rates[(int)Emotion.Anger])
        {
            // 怒り
            emotion = Emotion.Anger;
        }
        else
        {
            // 暴走
            emotion = Emotion.Lazy;
        }

        return emotion;
    }

    public void SetEmotion(Emotion _emotion)
    {
        // 外部から強制的に感情を設定
        emotion = _emotion;
    }
}
