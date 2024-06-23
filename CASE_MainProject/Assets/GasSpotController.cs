using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GasSpotController : MonoBehaviour
{

    [SerializeField, Header("�͈͐ݒ�I�u�W�F�N�g")]
    GameObject spawnAreaSettingObject;

    Vector3 spawnAreaMin, spawnAreaMax;

    [SerializeField, Header("�������锚���|�C���g")]
    GameObject explosionPoint;

    [SerializeField, Header("�����N�[���_�E���^�C��")]
    float spawnTime = 0.5f;

    [SerializeField, Header("�����N�[���_�E���^�C�}�["), ReadOnly]
    float spawnTimer = 0.0f;

    [SerializeField, Header("���ł̐�����")]
    int spawnCount = 2;

    [SerializeField, Header("�����L��")]
    bool bSpawnEnable = true;

    private void Awake()
    {
        //���s����Ə�����
        if(this.enabled)
            spawnAreaSettingObject.GetComponent<MeshRenderer>().enabled = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        //�����͈͂̌v�Z
        spawnAreaMin = spawnAreaSettingObject.transform.position - spawnAreaSettingObject.transform.localScale * 0.5f;
        spawnAreaMax = spawnAreaSettingObject.transform.position + spawnAreaSettingObject.transform.localScale * 0.5f;
    }

    // Update is called once per frame
    void Update()
    {
        if (bSpawnEnable)
        {
            spawnTimer += Time.deltaTime;
            if(spawnTimer > spawnTime)
            {
                //�ݒ��
                for (int i = 0; i < spawnCount; i++)
                {
                    //�����|�C���g�������W�������_���Ŏw��
                    Vector3 spawnPosition = new Vector3(
                        Random.Range(spawnAreaMin.x, spawnAreaMax.x),
                        Random.Range(spawnAreaMin.y, spawnAreaMax.y),
                        Random.Range(spawnAreaMin.z, spawnAreaMax.z)
                        );

                    //�����|�C���g����
                    Instantiate(explosionPoint, spawnPosition, Quaternion.identity);
                }

                //�^�C�}�[�̃��Z�b�g
                spawnTimer = 0.0f;
            }
        }
    }
}
