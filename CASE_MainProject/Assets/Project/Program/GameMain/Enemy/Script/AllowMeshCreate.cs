using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllowMeshCreate : MonoBehaviour
{
    [SerializeField, Header("���ʂ��Ȃ��邽�߂̃J�[�u"), AnimCurve]
    AnimationCurve planeCurve;

    [SerializeField, Header("�����J�n�ʒu")]
    Transform startTransform;
    [SerializeField, Header("�����I���ʒu")]
    Transform endTransform;

    [SerializeField, Header("���_")]
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
