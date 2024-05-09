using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField, Header("破壊までの時間")]
    float destroyDuration = 2.0f;
    [SerializeField, Header("カウント"), ReadOnly]
    float cnt = 0.0f;

    [SerializeField, Header("攻撃力")]
    float attackPower = 1.0f;

    private void Update()
    {
        cnt += Time.deltaTime;
        if(cnt >= destroyDuration)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.tag == "Player")
        {
            // ダメージ与える
        }
        Destroy(gameObject);
    }
}