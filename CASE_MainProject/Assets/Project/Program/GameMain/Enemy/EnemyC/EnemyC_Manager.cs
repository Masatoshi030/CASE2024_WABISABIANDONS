using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyC_Manager : MonoBehaviour
{
    public static EnemyC_Manager instance;
    [SerializeField, Header("�Փ˃G�t�F�N�g")]
    GameObject collisionEffect;
    [SerializeField, Header("�Փ�SE")]
    AudioSource collisionSE;

    [SerializeField, Header("�����G�t�F�N�g")]
    GameObject explosionEffect;
    [SerializeField, Header("����SE")]
    AudioSource explisionSE;

    private void Start()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public GameObject CreateCollisionEffect(Vector3 position, Quaternion quaternion)
    {
        GameObject obj = Instantiate(collisionEffect, position, quaternion);
        obj.GetComponent<AudioSource>().PlayOneShot(collisionSE.clip);
        return obj;
    }

    public GameObject CreateExplosionEffect(Vector3 position, Quaternion quaternion)
    {
        GameObject obj = Instantiate(explosionEffect, position, quaternion);
        obj.GetComponent<AudioSource>().PlayOneShot(explisionSE.clip);
        return obj;
    }
}
