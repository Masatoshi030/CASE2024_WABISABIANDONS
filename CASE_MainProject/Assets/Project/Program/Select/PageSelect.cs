using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PageSelect : MonoBehaviour
{
    //��`�̈恁����������������������������������
    [SerializeField, Header("�y�[�W�ݒ�")]
    GameObject[] papers;
    [SerializeField, Header("���\���L�[�G���Ă��Ȃ�����")]
    float noTouchTime;
    [SerializeField, Header("���\���L�[�̃N�[���^�C��")]
    float buttonCoolTime;

    [SerializeField, Header("���ʉ��Đ��\�[�X")]
    AudioSource myAudioSource;

    [SerializeField, Header("�y�[�W�߂��艹")]
    AudioClip pageSwitchSound;

    public static int serectPage;
    private Animator anim;

    // Start�֐�
    void Start()
    {

        serectPage = 0;
        anim= gameObject.GetComponent<Animator>();

        //�P�y�[�W�ȊO��\��
        papers[0].SetActive(true);
        papers[1].SetActive(false);
        papers[2].SetActive(false);
        papers[3].SetActive(false);
    }

    

    // Update�֐�
    void Update()
    {
        noTouchTime += Time.deltaTime;

        //�E�{�^������
        if (DualSense_Manager.instance.GetInputState().DPadRightButton == DualSenseUnity.ButtonState.Down)
        {
            //�N�[���^�C�����オ���Ă�����{�^�����͏���
            if (noTouchTime > buttonCoolTime && serectPage < 3)
            {
                //Bool�^�̃p�����[�^�[�ł���bPageMove��True�ɂ���
                anim.SetTrigger("tPageMove");

                //�y�[�W�؂�ւ����Đ�
                myAudioSource.PlayOneShot(pageSwitchSound);

                DualSense_Manager.instance.SetLeftRumble(0.1f, 0.1f);
                serectPage++;
            }
            //�{�^�����G��ꂽ�̂ŃN�[���^�C��������
            noTouchTime = 0.0f;
        }

        //���{�^������
        if (DualSense_Manager.instance.GetInputState().DPadLeftButton == DualSenseUnity.ButtonState.Down)
        {
            //�N�[���^�C�����オ���Ă�����{�^�����͏���
            if (noTouchTime > buttonCoolTime && serectPage > 0)
            {
                //Bool�^�̃p�����[�^�[�ł���bPageMove��True�ɂ���
                anim.SetTrigger("tPageMove");

                //�y�[�W�؂�ւ����Đ�
                myAudioSource.PlayOneShot(pageSwitchSound);

                DualSense_Manager.instance.SetLeftRumble(0.1f, 0.1f);
                serectPage--;
            }
            //�{�^�����G��ꂽ�̂ŃN�[���^�C��������
            noTouchTime = 0.0f;
        }
        //�y�[�W�ؑ֏���
        SelectPaperProcess();
    }

    private void SelectPaperProcess()
    {
        if (serectPage == 0)
        {
            papers[0].SetActive(true);
            papers[1].SetActive(false);
            papers[2].SetActive(false);
            papers[3].SetActive(false);
        }
        if (serectPage == 1)
        {
            papers[0].SetActive(false);
            papers[1].SetActive(true);
            papers[2].SetActive(false);
            papers[3].SetActive(false);
        }
        if (serectPage == 2)
        {
            papers[0].SetActive(false);
            papers[1].SetActive(false);
            papers[2].SetActive(true);
            papers[3].SetActive(false);
        }
        if (serectPage == 3)
        {
            papers[0].SetActive(false);
            papers[1].SetActive(false);
            papers[2].SetActive(false);
            papers[3].SetActive(true);
        }
    }
}
