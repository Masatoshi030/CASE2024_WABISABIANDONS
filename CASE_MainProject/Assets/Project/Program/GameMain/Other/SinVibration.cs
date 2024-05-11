using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinVibration : MonoBehaviour
{

    [SerializeField, Header("振動の大きさ"), Range(0.0f, 0.1f)]
    float Intensity = 0.1f;

    [SerializeField, Header("振動の速さ"), Range(0.0f, 100.0f)]
    float Speed = 1.0f;

    Vector3 StartPosition;

    // Start is called before the first frame update
    void Start()
    {
        StartPosition = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        transform.localPosition = StartPosition + transform.up * Mathf.Sin(Time.time * Speed) * Intensity;
    }
}
