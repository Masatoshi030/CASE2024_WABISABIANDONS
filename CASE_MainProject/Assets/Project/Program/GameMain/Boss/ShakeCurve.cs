using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeCurve : MonoBehaviour
{
    [SerializeField, Header("�^�C�����[�g")]
    float timeRate = 1.0f;
    [SerializeField, Header("�ړ����[�g")]
    float moveRate = 1.0f;


    float cnt;
    float Originheight = 0.0f;

    [SerializeField, Header("�J�[�u"), AnimCurve]
    AnimationCurve shakeCurve;


    private void Start()
    {
        cnt = 0.0f;
        Originheight = transform.position.y;
    }

    private void Update()
    {
        cnt += Time.deltaTime * timeRate;
        float shake = shakeCurve.Evaluate(cnt) * moveRate;

        Vector3 newPosition = transform.position;
        newPosition.y = Originheight + shake;
        transform.position = newPosition;
    }
}
