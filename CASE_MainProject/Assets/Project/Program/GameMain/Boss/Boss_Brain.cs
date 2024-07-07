using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Brain : MonoBehaviour
{
    enum Emotion
    {
        Anger,
        Calm,
        Lazy,
        Max
    }

    [SerializeField, Header("ä¥èÓîzóÒ")]
    int[] emotionArray;
    float[] fEmotionArray;

    // Start is called before the first frame update
    void Start()
    {
        emotionArray = new int[(int)Emotion.Max];
        fEmotionArray = new float[(int)Emotion.Max];
    }

    // Update is called once per frame
    void Update()
    {
        int total = 0;

        for(int i = 0; i < (int)Emotion.Max; i++)
        {
            total += emotionArray[i];
        }


    }
}
