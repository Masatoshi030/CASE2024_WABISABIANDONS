using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetAllow : MonoBehaviour
{
    [SerializeField, Header("LineRenderer")]
    LineRenderer lineRenderer;

    [SerializeField, Header("分割数")]
    int separateNum;

    [SerializeField, Header("曲線"), AnimCurve]
    AnimationCurve curve;
    [SerializeField, Header("曲線の強さ")]
    float curveRate;

    [SerializeField, Header("開始位置")]
    Transform startTransform;
    [SerializeField, Header("終了位置")]
    Transform endTransform;
    Vector3[] vertex;

    [SerializeField, Header("ターゲットがプレイヤーか?")]
    bool isTargetPlayer = false;

    private void Start()
    {
        vertex = new Vector3[separateNum + 2];
        if(isTargetPlayer)
        {
            endTransform = GameObject.Find("Player").transform;
        }
    }

    private void Update()
    {
        float rate = 1.0f / (separateNum + 1);

        vertex[0] = startTransform.position;
        for(int i = 1; i < separateNum+1; i++)
        {
            // 補間で位置を補う
            Vector3 position = Vector3.Lerp(startTransform.position, endTransform.position, rate * i);
            float yGap = curve.Evaluate(rate * i) * curveRate;
            position.y += yGap;
            vertex[i] = position;
        }
        vertex[separateNum + 1] = endTransform.position;

        lineRenderer.positionCount = separateNum + 2;
        lineRenderer.SetPositions(vertex);
    }
}