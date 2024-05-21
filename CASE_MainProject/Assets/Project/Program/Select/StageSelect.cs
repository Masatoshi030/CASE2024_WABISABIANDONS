using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Threading;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class StageSelect : MonoBehaviour
{
    [SerializeField, Header("�N���A�X�e�[�W�̐�")]
    int checkNum;
    [SerializeField, Header("�N�[���^�C�����ԕۑ�")]
    float timeElapsed;
    [SerializeField, Header("UP,DOWN�{�^���������Z���N�g�N�[���^�C��")]
    float timeOut;
    [SerializeField, Header("UP,DOWN�G���Ă��Ȃ�����")]
    float noTouchTime;
    [SerializeField, Header("UP,DOWN�{�^���̃N�[���^�C��")]
    float buttonCoolTime;
    [SerializeField, Header("�y�[�W�ݒ�")]
    GameObject[] papers;

    //��`
    static int clearPage;                //���݂̃N���A�y�[�W
    static int clearSelectPage;    //���݂̃N���A�Z���N�g��
    static int nowPage;     //���̃y�[�W
    static int nowSelect;  //���̃Z���N�g�X�e�[�W
    static int maxPage = 4;    //�y�[�W�̍ő�
    static int minPage = 0;     //�y�[�W�̍ŏ�
    static int maxSelectStage = 6;     //�X�e�[�W�̍ő�
    static int minSelectStage = 0;     //�X�e�[�W�̍ŏ�

    public GameObject[,] stageArray = new GameObject[4, 6];     //�S�X�e�[�W���̔z��
    public GameObject[,] checkArray = new GameObject[4, 6];     //�X�e�[�W�̃N���A�󋵊m�F�z��

    // Start�֐�
    void Start()
    {
        //�X�e�[�W�̃Z���N�gUI�i�[
        for (int i = minPage; i < maxPage; i++)
        {
            for (int j = minSelectStage; j < maxSelectStage; j++)
            {
                stageArray[i, j] = papers[i].transform.GetChild(j).transform.GetChild(1).gameObject;
                checkArray[i, j] = papers[i].transform.GetChild(j).transform.GetChild(0).gameObject;
                checkArray[i, j].SetActive(true);
            }
        }
    }

    // Update�֐�
    void Update()
    {
        //�y�[�W�Z���N�g�X�N���v�g���獡�̃y�[�W�������Ă���
        nowPage = PageSelect.serectPage;

        //�G���Ă��Ȃ����Ԋi�[
        noTouchTime += Time.deltaTime;

        //���{�^������
        if (DualSense_Manager.instance.GetInputState().DPadDownButton == DualSenseUnity.ButtonState.Down)
        {
            //�N�[���^�C�����オ���Ă�����{�^�����͏���
            if (noTouchTime > buttonCoolTime)
            {
                DualSense_Manager.instance.SetLeftRumble(0.1f, 0.04f);
                nowSelect++;
                timeElapsed = 0.0f;
            }
            timeElapsed += Time.deltaTime;

            //��莞�ԉ����Ă���Ǝ��s����鏈��
            if (timeElapsed > timeOut)
            {
                DualSense_Manager.instance.SetLeftRumble(0.1f, 0.04f);
                nowSelect++;
                timeElapsed = 0.0f;
            }

            //����ɒB�������ɖ߂鏈��
            if (nowSelect > maxSelectStage - 1)
            {
                nowSelect = minSelectStage;
            }

            //�{�^�����G��ꂽ�̂ŃN�[���^�C��������
            noTouchTime = 0.0f;
        }

        //��{�^������
        if (DualSense_Manager.instance.GetInputState().DPadUpButton == DualSenseUnity.ButtonState.Down)
        {
            //�N�[���^�C�����オ���Ă�����{�^�����͏���
            if (noTouchTime > buttonCoolTime)
            {
                DualSense_Manager.instance.SetLeftRumble(0.1f, 0.04f);
                nowSelect--;
                timeElapsed = 0.0f;
            }

            timeElapsed += Time.deltaTime;

            //��莞�ԉ����Ă���Ǝ��s����鏈��
            if (timeElapsed > timeOut)
            {
                DualSense_Manager.instance.SetLeftRumble(0.1f, 0.04f);

                nowSelect--;
                timeElapsed = 0.0f;
            }
            //����ɒB�����牺�ɖ߂鏈��
            if (nowSelect < minSelectStage)
            {
                nowSelect = maxSelectStage - 1;
            }

            //�{�^�����G��ꂽ�̂ŃN�[���^�C��������
            noTouchTime = 0.0f;
        }

        //�Z���N�g����Ă��Ȃ���Ԃɂ��鏈��
        for (int i = minPage; i < maxPage; i++)
        {
            for (int j = minSelectStage; j < maxSelectStage; j++)
            {
                if (stageArray[i, j] != null)
                {
                    stageArray[i, j].SetActive(false);
                }
            }
        }
        //�Z���N�g����Ă���{�^����Active�ɂ��鏈��
        SelectActive(nowPage, nowSelect);

        //�`�F�b�N��\������
        //�`�F�b�N���S�̂̃X�e�[�W�����z���Ă��Ȃ������肷�鏈��
        if (checkNum >= 0 && maxPage * maxSelectStage >= checkNum)
        {
            clearPage = checkNum / maxSelectStage;
            clearSelectPage = checkNum % maxSelectStage;

            //�z���Ă���ꍇ(�N���A�X�e�[�W��6�ȏ�)
            if (clearPage > 0)
            {
                for (int i = 0; i < clearPage; i++)
                {
                    for (int j = 0; j < 6; j++)
                    {
                        checkArray[i, j].GetComponent<Animator>().SetBool("bCheck", true);
                        checkArray[i, j].SetActive(true);
                    }
                    for (int j = 0; j < clearSelectPage; j++)
                    {
                        checkArray[clearPage, j].GetComponent<Animator>().SetBool("bCheck", true);
                        checkArray[clearPage, j].SetActive(true);
                    }
                }
            }
            else  //�z���Ă��Ȃ��ꍇ(�N���A�X�e�[�W��6�ȏ�)
            {
                for (int i = 0; i < clearSelectPage; i++)
                {
                    checkArray[clearPage, i].GetComponent<Animator>().SetBool("bCheck", true);
                    checkArray[clearPage, i].SetActive(true);
                }
            }
        }
        else if (checkNum > 24)     //24�X�e�[�W�ȏ��I�����Ă��܂����ꍇ
        {
            checkNum = 24;
        }
    }

    //�Z���N�g����Ă���t���[����Active�ɂ��鏈��
    private void SelectActive(int _nowPage,int _nowSelect)
    {
        stageArray[_nowPage, _nowSelect].SetActive(true);
    }
}
