using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using TMPro;

public class ExplosionElementController : MonoBehaviour
{

    [SerializeField, Header("�����G�t�F�N�g")]
    GameObject explosionObject;

    [SerializeField, Header("��������܂ł̎���")]
    float explosionOffsetTime = 0.5f;

    [SerializeField, Header("��������܂ł̃^�C�}�[")]
    float explosionOffsetTimer = 0.0f;

    [SerializeField, Header("�������Ă��画��𖳌��ɂ���܂ł̎���")]
    float explosionAfterDisableTime = 0.2f;

    [SerializeField, Header("�������Ă��画��𖳌��ɂ���܂ł̃^�C�}�[")]
    float explosionAfterDisableTimer = 0.0f;

    [SerializeField, Header("�������o��")]
    int explosionCount = 0;

    [SerializeField, Header("����")]
    bool bExplosion = false;

    [SerializeField, Header("�����G�t�F�N�g�����ς�")]
    bool bExplosionInstantiated = false;


    private void Update()
    {
        if (bExplosionInstantiated)
        {
            //�^�C�}�[�J�E���g
            explosionOffsetTimer += Time.deltaTime;

            if (explosionOffsetTimer > explosionOffsetTime)
            {
                bExplosion = true;
            }
        }

        if (bExplosion)
        {
            //�����㔻�薳���^�C�}�[�J�E���g
            explosionAfterDisableTimer += Time.deltaTime;

            if (explosionAfterDisableTimer > explosionAfterDisableTime)
            {
                this.GetComponent<SphereCollider>().enabled = false;
            }
        }
    }

    public void SetExplosion()
    {
        if (!bExplosionInstantiated)
        {
            //�����_����]�p�x�Ŕ����G�t�F�N�g�𐶐�
            GameObject effect = Instantiate(explosionObject, transform.position, Quaternion.identity);

            //�q�ǂ��ɂ���
            effect.transform.parent = this.transform;

            //���������ς�
            bExplosionInstantiated = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (bExplosion)
        {
            if (other.tag == "Explosion")
            {
                other.GetComponent<ExplosionElementController>().SetExplosion();
            }
        }
    }
}
