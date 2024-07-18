using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectScaler : MonoBehaviour
{
    [SerializeField, Header("最大スケール")]
    float maxScale = 1.05f;
    [SerializeField, Header("最小スケール")]
    float minScale = 0.95f;
    [SerializeField, Header("スケールスピード")]
    float scaleSpeed = 3.0f;

    Vector3 initialScale;

    float scalePerSecond;

    float count = 0.0f;

    float currentScale = 1.0f;

    bool bScaling = true;

    bool bBigger = true;

    private void Start()
    {
        initialScale = transform.localScale;
        scalePerSecond = (maxScale - minScale) / scaleSpeed;
        currentScale = 1.0f;
    }

    private void Update()
    {
        if(bScaling)
        {
            if(bBigger)
            {
                currentScale += scalePerSecond * Time.deltaTime;
                if (currentScale > maxScale)
                {
                    currentScale = maxScale;
                    bBigger = false;
                }
            }
            else
            {
                currentScale -= scalePerSecond * Time.deltaTime;
                if (currentScale < minScale)
                {
                    currentScale = minScale;
                    bBigger = true;
                }
            }
            Vector3 scale = initialScale * currentScale;
            transform.localScale = scale;
        }
    }



}
