using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Manager : MonoBehaviour
{
    public static Enemy_Manager instance;
    [SerializeField, Header("ÎÔGtFNg")]
    GameObject sparkleEffect;
    [SerializeField, Header("ÎÔSE")]
    AudioSource sparkleSE;
    [SerializeField, Header("ÕËGtFNg")]
    GameObject collisionEffect;
    [SerializeField, Header("ÕËSE")]
    AudioSource collisionSE;
    [SerializeField, Header("­GtFNg")]
    GameObject explosionEffect;
    [SerializeField, Header("­SE")]
    AudioSource explisionSE;
    [SerializeField, Header("öCGtFNg")]
    GameObject steamEffect;
    [SerializeField, Header("öCSE")]
    AudioSource steamSE;

    [SerializeField, Header("½Ë}eA0")]
    Material reflectMaterial0;
    public Material Material0 { get => reflectMaterial0; }

    [SerializeField, Header("½Ë}eA1")]
    Material reflectMaterial1;
    public Material Material1 { get => reflectMaterial1; }

    [SerializeField, Header("½Ë}eA2")]
    Material reflectMaterial2;
    public Material Material2 { get => reflectMaterial2; }

    [SerializeField, Header("|µ½")]
    uint DefeatEnemyNum = 0;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DefeatEnemyNum = 0;
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

    public GameObject CreateSteamEffect(Vector3 position, Quaternion quaternion)
    {
        GameObject obj = Instantiate(steamEffect, position, quaternion);
        obj.GetComponent<AudioSource>().PlayOneShot(steamSE.clip);
        return obj;
    }
    
    public GameObject CreateSparkleEffect(Vector3 position, Quaternion quaternion)
    {
        GameObject obj = Instantiate(sparkleEffect, position, quaternion);
        obj.GetComponent<AudioSource>().PlayOneShot(sparkleSE.clip);
        return obj;
    }

    public void AddDefeatEnemy()
    {
        DefeatEnemyNum++;
    }

    public uint GetDefeatEnemyNum()
    {
        return DefeatEnemyNum;
    }


    public void DestroyInstance()
    {
        Destroy(this);
    }
}
