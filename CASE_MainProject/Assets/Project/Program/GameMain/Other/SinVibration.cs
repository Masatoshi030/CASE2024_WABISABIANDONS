using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinVibration : MonoBehaviour
{

    [SerializeField, Header("�U���̑傫��"), Range(0.0f, 10.0f)]
    public float Intensity = 0.1f;

    [SerializeField, Header("�U���̑���"), Range(0.0f, 100.0f)]
    public float Speed = 1.0f;

    [SerializeField, Header("�X�P�[�����[�h�L��")]
    bool bScale = false;

    Vector3 StartPosition;

    Vector3 StartScale;

    // Start is called before the first frame update
    void Start()
    {
        StartPosition = transform.localPosition;
        StartScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        if (bScale)
        {
            float scaleValue = Mathf.Sin(Time.time * Speed) * Intensity;
            transform.localScale = StartScale + new Vector3(scaleValue, scaleValue, scaleValue);
        }
        else
        {
            transform.localPosition = StartPosition + transform.up * Mathf.Sin(Time.time * Speed) * Intensity;
        }
    }
}
