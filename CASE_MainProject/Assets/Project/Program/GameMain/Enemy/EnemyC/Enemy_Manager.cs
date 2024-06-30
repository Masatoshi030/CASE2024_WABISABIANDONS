using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Manager : MonoBehaviour
{
    public static Enemy_Manager instance;
    [SerializeField, Header("�ΉԃG�t�F�N�g")]
    GameObject sparkleEffect;
    [SerializeField, Header("�Ή�SE")]
    AudioSource sparkleSE;
    [SerializeField, Header("�Փ˃G�t�F�N�g")]
    GameObject collisionEffect;
    [SerializeField, Header("�Փ�SE")]
    AudioSource collisionSE;
    [SerializeField, Header("�����G�t�F�N�g")]
    GameObject explosionEffect;
    [SerializeField, Header("����SE")]
    AudioSource explisionSE;
    [SerializeField, Header("���C�G�t�F�N�g")]
    GameObject steamEffect;
    [SerializeField, Header("���CSE")]
    AudioSource steamSE;

    [SerializeField, Header("���˃}�e���A��0")]
    Material reflectMaterial0;
    public Material Material0 { get => reflectMaterial0; }

    [SerializeField, Header("���˃}�e���A��1")]
    Material reflectMaterial1;
    public Material Material1 { get => reflectMaterial1; }

    [SerializeField, Header("���˃}�e���A��2")]
    Material reflectMaterial2;
    public Material Material2 { get => reflectMaterial2; }

    [SerializeField, Header("�|������")]
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
