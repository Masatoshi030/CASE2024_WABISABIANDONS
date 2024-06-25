using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllowMeshCreate : MonoBehaviour
{
    [SerializeField, Header("平面を曲げるためのカーブ"), AnimCurve]
    AnimationCurve planeCurve;

    [SerializeField, Header("生成開始位置")]
    Transform startTransform;
    [SerializeField, Header("生成終了位置")]
    Transform endTransform;

    [SerializeField, Header("頂点")]
    Vector3[] vertices;

    [SerializeField, Header("LineRenderer")]
    LineRenderer lineRenderer;

    void Start()
    {
        vertices = new Vector3[3];
        lineRenderer = GetComponent<LineRenderer>();
    }

    void Update()
    {
        Vector3 half = (startTransform.position + endTransform.position) / 2;
        half.y += 0.5f;
        vertices[0] = startTransform.position;
        vertices[1] = half;
        vertices[2] = endTransform.position;

        lineRenderer.positionCount = 3;

        lineRenderer.SetPositions(vertices);
    }
}
