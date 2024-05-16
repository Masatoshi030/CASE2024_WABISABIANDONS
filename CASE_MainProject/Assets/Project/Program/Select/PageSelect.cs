using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PageSelect : MonoBehaviour
{
    //��`�̈恁����������������������������������
    [Header("���݂̃y�[�W")]
    public static int serectPage;

    [SerializeField, Header("�y�[�W1")]
    GameObject paper_1;
    [SerializeField, Header("�y�[�W2")]
    GameObject paper_2;
    [SerializeField, Header("�y�[�W3")]
    GameObject paper_3;
    [SerializeField, Header("�y�[�W4")]
    GameObject paper_4;


    [SerializeField, Header("R2L2�G���Ă��Ȃ�����")]
    float noTouchTime;
    [SerializeField, Header("R2L2�{�^���̃N�[���^�C��")]
    float buttonCoolTime;

    private Animator anim;

    // Start�֐�
    void Start()
    {
        serectPage = 0;
        anim= gameObject.GetComponent<Animator>();

        //�P�y�[�W�ȊO��\��
        paper_1.SetActive(true);
        paper_2.SetActive(false);
        paper_3.SetActive(false);
        paper_4.SetActive(false);
    }

    

    // Update�֐�
    void Update()
    {
        noTouchTime += Time.deltaTime;

        if (DualSense_Manager.instance.GetInputState().RightTrigger.TriggerValue > 0.01f &&
            1.0f <= DualSense_Manager.instance.GetInputState().RightTrigger.TriggerValue)
        {
            //�N�[���^�C�����オ���Ă�����{�^�����͏���
            if (noTouchTime > buttonCoolTime && serectPage < 3)
            {
                //Bool�^�̃p�����[�^�[�ł���bPageMove��True�ɂ���
                anim.SetTrigger("tPageMove");

                DualSense_Manager.instance.SetLeftRumble(0.1f, 0.04f);
                serectPage++;
                //timeElapsed = 0.0f;
            }
            //timeElapsed += Time.deltaTime;
            //�{�^�����G��ꂽ�̂ŃN�[���^�C��������
            noTouchTime = 0.0f;
        }

        if (DualSense_Manager.instance.GetInputState().LeftTrigger.TriggerValue > 0.01f &&
           1.0f <= DualSense_Manager.instance.GetInputState().LeftTrigger.TriggerValue)
        {
            //�N�[���^�C�����オ���Ă�����{�^�����͏���
            if (noTouchTime > buttonCoolTime && serectPage > 0)
            {
                //Bool�^�̃p�����[�^�[�ł���bPageMove��True�ɂ���
                anim.SetTrigger("tPageMove");

                DualSense_Manager.instance.SetLeftRumble(0.1f, 0.04f);
                serectPage--;
                //timeElapsed = 0.0f;
            }
            //timeElapsed += Time.deltaTime;
            //�{�^�����G��ꂽ�̂ŃN�[���^�C��������
            noTouchTime = 0.0f;
        }



        SelectPaperProcess();
    }

    private void SelectPaperProcess()
    {
        if (serectPage == 0)
        {
            paper_1.SetActive(true);
            paper_2.SetActive(false);
            paper_3.SetActive(false);
            paper_4.SetActive(false);
        }
        if (serectPage == 1)
        {
            paper_1.SetActive(false);
            paper_2.SetActive(true);
            paper_3.SetActive(false);
            paper_4.SetActive(false);
        }
        if (serectPage == 2)
        {
            paper_1.SetActive(false);
            paper_2.SetActive(false);
            paper_3.SetActive(true);
            paper_4.SetActive(false);
        }
        if (serectPage == 3)
        {
            paper_1.SetActive(false);
            paper_2.SetActive(false);
            paper_3.SetActive(false);
            paper_4.SetActive(true);
        }
    }
}
