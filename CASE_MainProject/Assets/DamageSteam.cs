using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageSteam : MonoBehaviour
{

    [SerializeField, Header("���C�G�t�F�N�g")]
    ParticleSystem steamEffect;

    [SerializeField, Header("�����蔻��")]
    Collider steamCollider;

    [SerializeField, Header("���o��")]
    bool bSteam = true;

    [SerializeField, Header("�C���^�[�o���L��")]
    bool bIntervalDisable = true;

    [SerializeField, Header("�C���^�[�o������")]
    float intervalTime = 3.0f;

    [SerializeField, Header("���o����")]
    float steamTime = 2.0f;

    [SerializeField, Header("�^�C�}�[")]
    float timer = 0.0f;

    [SerializeField, Header("�o���u�̐U��")]
    SinVibration mySinVibration;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //�C���^�[�o���@�\���L����������
        if (bIntervalDisable)
        {
            //�^�C�}�[��i�߂�
            timer += Time.deltaTime;

            if (bSteam)
            {
                if(timer > steamTime)
                {
                    //���o��~���
                    bSteam = false;

                    //�G�t�F�N�g���~�߂�
                    steamEffect.Stop();

                    //�����蔻��𖳌��ɂ���
                    steamCollider.enabled = false;

                    //�U���J�n
                    mySinVibration.enabled = true;

                    //�^�C�}�[�����Z�b�g
                    timer = 0.0f;
                }
            }
            else
            {
                if (timer > intervalTime)
                {
                    //���o���
                    bSteam = true;

                    //�G�t�F�N�g���Đ�����
                    steamEffect.Play();

                    //�����蔻���L���ɂ���
                    steamCollider.enabled = true;

                    //�U���I��
                    mySinVibration.enabled = false;

                    timer = 0.0f;
                }
            }
        }
        else
        {

        }
    }
}
