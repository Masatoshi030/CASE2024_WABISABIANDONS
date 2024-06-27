using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetAllow : MonoBehaviour
{
    [SerializeField, Header("LineRenderer")]
    LineRenderer lineRenderer;

    [SerializeField, Header("������")]
    int separateNum;

    [SerializeField, Header("�Ȑ�"), AnimCurve]
    AnimationCurve curve;
    [SerializeField, Header("�Ȑ��̋���")]
    float curveRate;

    [SerializeField, Header("�J�n�ʒu")]
    Transform startTransform;
    [SerializeField, Header("�I���ʒu")]
    Transform endTransform;
    Vector3[] vertex;

    [SerializeField, Header("�^�[�Q�b�g���v���C���[��?")]
    bool isTargetPlayer = false;

    [SerializeField, Header("�|�W�V�����w�肩")]
    bool isDesignationPositions = false;
    Vector3 startPosition;
    Vector3 endPosition;

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
        if(!isDesignationPositions)
        {
            float rate = 1.0f / (separateNum + 1);

            vertex[0] = startTransform.position;
            for (int i = 1; i < separateNum + 1; i++)
            {
                // ��Ԃňʒu��₤
                Vector3 position = Vector3.Lerp(startTransform.position, endTransform.position, rate * i);
                float yGap = curve.Evaluate(rate * i) * curveRate;
                position.y += yGap;
                vertex[i] = position;
            }
            vertex[separateNum + 1] = endTransform.position;

            lineRenderer.positionCount = separateNum + 2;
            lineRenderer.SetPositions(vertex);
        }
        // �n�_�ƏI�_���w�肷��
        else
        {
            float rate = 1.0f / (separateNum + 1);

            vertex[0] = startPosition;
            for (int i = 1; i < separateNum + 1; i++)
            {
                // ��Ԃňʒu��₤
                Vector3 position = Vector3.Lerp(startPosition, endPosition, rate * i);
                float yGap = curve.Evaluate(rate * i) * curveRate;
                position.y += yGap;
                vertex[i] = position;
            }
            vertex[separateNum + 1] = endPosition;

            lineRenderer.positionCount = separateNum + 2;
            lineRenderer.SetPositions(vertex);
        }
    }

    public void StartDesignation(Vector3 start, Vector3 end)
    {
        startPosition = start;
        endPosition = end;
        isDesignationPositions = true;
    }

    public void EndDesignation()
    {
        isDesignationPositions = false;
    }

    public void SetMaterial(Material material)
    {
        lineRenderer.material = material;
    }
}