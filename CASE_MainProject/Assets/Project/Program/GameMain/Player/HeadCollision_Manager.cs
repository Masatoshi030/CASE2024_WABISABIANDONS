using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class HeadCollision_Manager : MonoBehaviour
{
    [SerializeField, Header("�I�[�f�B�I�\�[�X�Q�Ɛ�")]
    AudioSource audioSource;

    [SerializeField, Header("�����N���b�v���X�g")]
    AudioClip[] soundClips;

    [SerializeField, Header("�ΉԃG�t�F�N�g")]
    GameObject hibana_ParticleEffect;

    [SerializeField, Header("�p�[�c�U�J�G�t�F�N�g")]
    GameObject partsSplit_ParticleEffect;

    [SerializeField, Header("�R�K�X���Δ���")]
    GameObject explosionSwitchObject;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Wall" || other.tag == "Ground" || other.tag == "Enemy" || other.tag == "Dummy" || other.tag == "Valve" || other.tag == "Goal" || other.tag == "BrokenWall")
        {
            if (PlayerController.instance.attackState == PlayerController.ATTACK_STATE.Attack)
            {
                //�G�ɓˌ��������̏���
                if (other.tag == "Enemy")
                {
                    Debug.Log("head");
                    Enemy enemy = other.GetComponent<Enemy>();

                    // ���S�t���O�̔���
                    bool isDeath = enemy.Damage(20.0f, transform.up);

                    // �G�l�~�[�̃g���K�[���Ăяo��
                    enemy.Machine.TriggerEnterOpponent(other);

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

                    //�ˌ��\�t���O��L���ɂ���
                    PlayerController.instance.bAttackPossible = true;

                    //�|������
                    if (isDeath)
                    {
                        //�p�[�c�U�J�G�t�F�N�g�𐶐�����
                        Instantiate(partsSplit_ParticleEffect, transform.position, Quaternion.identity);

                        //�q�b�g�X�g�b�v
                        HitStopManager.instance.HitStopEffect(0.5f, 0.25f);

                        //���U��
                        DualSense_Manager.instance.SetRumble_Type1();
                    }
                    //�|���ĂȂ�
                    else
                    {
                        //�q�b�g�X�g�b�v
                        HitStopManager.instance.HitStopEffect(0.2f, 0.25f);

                        //���U��
                        DualSense_Manager.instance.SetLeftRumble(0.75f, 0.1f);
                    }
                }
                // �M�~�b�N�Փˎ��̏���
                if (other.tag == "Valve")
                {
                    other.GetComponent<Valve_Base>().SetCommand();
                }

                if (other.tag == "BrokenWall")
                {
                    if (other.transform.parent.GetComponent<BrokenWallController>().bBroken == false)
                    {
                        if (PlayerController.instance.heldSteam / PlayerController.instance.maxHeldSteam > 0.9f)
                        {
                            other.transform.parent.GetComponent<BrokenWallController>().SetBreak();

                            //�q�b�g�X�g�b�v
                            HitStopManager.instance.HitStopEffect(0.05f, 0.5f);

                            //���U��
                            DualSense_Manager.instance.SetLeftRumble(1.0f, 0.1f);

                            //�Փˉ�
                            audioSource.PlayOneShot(soundClips[1]);

                            //�ΉԂ𐶐�����
                            Instantiate(hibana_ParticleEffect, transform.position, Quaternion.identity);

                            return;
                        }
                    }
                }

                if (other.tag == "Goal")
                {
                    //�S�[���̃A�j���[�V�������ڍs
                    other.GetComponent<Animator>().SetBool("bGoal", true);

                    //�v���C���[�̃S�[������
                    PlayerController.instance.OnGoal();
                }

                //���Δ��萶��
                if (other.tag == "Wall" || other.tag == "Ground")
                {
                    Instantiate(explosionSwitchObject, transform.position, Quaternion.identity);
                }

                //�ˌ��I��
                PlayerController.instance.StopAttack();

                //�Փˉ�
                audioSource.PlayOneShot(soundClips[1]);

                //�ΉԂ𐶐�����
                Instantiate(hibana_ParticleEffect, transform.position, Quaternion.identity);

                //���U��
                DualSense_Manager.instance.SetLeftRumble(0.25f, 0.1f);
            }
        }
    }
}