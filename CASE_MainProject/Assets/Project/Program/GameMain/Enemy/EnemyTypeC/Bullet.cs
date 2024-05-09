using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField, Header("�j��܂ł̎���")]
    float destroyDuration = 2.0f;
    [SerializeField, Header("�J�E���g"), ReadOnly]
    float cnt = 0.0f;

    [SerializeField, Header("�U����")]
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
            // �_���[�W�^����
        }
        Destroy(gameObject);
    }
}