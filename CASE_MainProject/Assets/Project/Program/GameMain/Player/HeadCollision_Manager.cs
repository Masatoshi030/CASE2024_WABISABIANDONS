using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadCollision_Manager : MonoBehaviour
{
    AudioSource audioSource;

    [SerializeField, Header("�����N���b�v���X�g")]
    AudioClip[] soundClips;

    [SerializeField, Header("�ΉԃG�t�F�N�g")]
    GameObject hibana_ParticleEffect;

    [SerializeField, Header("�p�[�c�U�J�G�t�F�N�g")]
    GameObject partsSplit_ParticleEffect;


    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Wall" || other.tag == "Ground" || other.tag == "Enemy" || other.tag == "Dummy")
        {
            if (PlayerController.instance.attackState == PlayerController.ATTACK_STATE.Attack)
            {
                //�ˌ��I��
                PlayerController.instance.StopAttack();

                //�Փˉ�
                audioSource.PlayOneShot(soundClips[1]);

                //�ΉԂ𐶐�����
                Instantiate(hibana_ParticleEffect, transform.position, Quaternion.identity);

                //���U��
                DualSense_Manager.instance.SetLeftRumble(0.25f, 0.1f);

                //�G�ɓˌ��������̏���
                if (other.tag == "Dummy")
                {
                    //�Փˋ�����
                    audioSource.PlayOneShot(soundClips[0]);

                    //�m�b�N�o�b�N
                    PlayerController.instance.KnockBack();

                    //�p�[�c�U�J�G�t�F�N�g�𐶐�����
                    Instantiate(partsSplit_ParticleEffect, transform.position, Quaternion.identity);

                    //�q�b�g�X�g�b�v
                    HitStopManager.instance.HitStopEffect(0.5f, 0.25f);

                    //���U��
                    DualSense_Manager.instance.SetRumble_Type1();

                    Destroy(other.gameObject);

                    //�|������
                    //if (other.GetComponent<Enemy_Mob>().Damage(20.0f, transform.up))
                    //{
                    //    //�p�[�c�U�J�G�t�F�N�g�𐶐�����
                    //    Instantiate(partsSplit_ParticleEffect, transform.position, Quaternion.identity);
                    //
                    //    //�q�b�g�X�g�b�v
                    //    HitStopManager.instance.HitStopEffect(0.5f, 0.25f);
                    //
                    //    //���U��
                    //    DualSense_Manager.instance.SetRumble_Type1();
                    //}
                    //�|���ĂȂ�
                    //else
                    //{
                    //    //�q�b�g�X�g�b�v
                    //    HitStopManager.instance.HitStopEffect(0.2f, 0.25f);
                    //
                    //    //���U��
                    //    DualSense_Manager.instance.SetLeftRumble(0.75f, 0.1f);
                    //}
                }
            }
        }

        if(other.tag == "Goal")
        {
            //�S�[���̃A�j���[�V�������ڍs
            other.GetComponent<Animator>().SetBool("bGoal", true);

            //�v���C���[�̃S�[������
            PlayerController.instance.OnGoal();
        }
    }
}