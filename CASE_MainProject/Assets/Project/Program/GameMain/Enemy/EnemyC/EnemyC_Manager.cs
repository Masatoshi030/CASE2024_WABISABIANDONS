using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyC_Manager : MonoBehaviour
{
    public static EnemyC_Manager instance;
    [SerializeField, Header("衝突エフェクト")]
    GameObject collisionEffect;
    [SerializeField, Header("衝突SE")]
    AudioSource collisionSE;

    [SerializeField, Header("爆発エフェクト")]
    GameObject explosionEffect;
    [SerializeField, Header("爆発SE")]
    AudioSource explisionSE;

    [SerializeField, Header("反射マテリアル0")]
    Material reflectMaterial0;
    public Material Material0 { get => reflectMaterial0; }

    [SerializeField, Header("反射マテリアル1")]
    Material reflectMaterial1;
    public Material Material1 { get => reflectMaterial1; }

    [SerializeField, Header("反射マテリアル2")]
    Material reflectMaterial2;
    public Material Material2 { get => reflectMaterial2; }

    private void Awake()
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
