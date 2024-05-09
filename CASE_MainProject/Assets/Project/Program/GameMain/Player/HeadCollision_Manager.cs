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
        if (other.tag == "Wall" || other.tag == "Ground" || other.tag == "Enemy")
        {
            if (PlayerController.instance.attackState == PlayerController.ATTACK_STATE.Attack)
            {
                //�ˌ��I��
                PlayerController.instance.StopAttack();

                //�Փˉ�
                audioSource.PlayOneShot(soundClips[1]);

                //�ΉԂ𐶐�����
                Instantiate(hibana_ParticleEffect, transform.position, Quaternion.identity);

                //�G�ɓˌ��������̏���
                if (other.tag == "Enemy")
                {
                    //�Փˋ�����
                    audioSource.PlayOneShot(soundClips[0]);

                    //�_���[�W��^����
                    other.GetComponent<Enemy_Mob>().Damage(20.0f, transform.up);

                    //�m�b�N�o�b�N
                    PlayerController.instance.KnockBack();
                }
            }
        }
    }
}