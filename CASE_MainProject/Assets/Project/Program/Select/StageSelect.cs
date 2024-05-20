using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

public class StageSelect : MonoBehaviour
{
    [SerializeField, Header("�N�[���^�C�����ԕۑ�")]
    float timeElapsed;
    [SerializeField, Header("UP,DOWN�{�^���������Z���N�g�N�[���^�C��")]
    float timeOut;
    [SerializeField, Header("UP,DOWN�G���Ă��Ȃ�����")]
    float noTouchTime;
    [SerializeField, Header("UP,DOWN�{�^���̃N�[���^�C��")]
    float buttonCoolTime;


    [SerializeField, Header("�y�[�W1")]
    GameObject[] papers;

    //��`
    int nowPage;    //���̃y�[�W
    int nowSelect;  //���̃Z���N�g�X�e�[�W
    int maxPage = 4;    //�y�[�W�̍ő�
    int minPage = 0;    //�y�[�W�̍ŏ�
    int maxSelectStage = 6;     //�X�e�[�W�̍ő�
    int minSelectStage = 0;     //�X�e�[�W�̍ŏ�
    public GameObject[,] stageArray = new GameObject[4, 6];     //�S�X�e�[�W���̔z��

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 6; j++)
            {
                stageArray[i, j] = papers[i].transform.GetChild(j).gameObject;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Active��ԈȊO�͔�\���ɂ��鏈��
        for (int i = minPage; i < maxPage; i++)
        {
            for (int j = minSelectStage; j < maxSelectStage; j++)
            {
                if (stageArray[i, j] != null)
                {

                }
            }
        }

        //�y�[�W�Z���N�g�X�N���v�g���獡�̃y�[�W�������Ă���
        nowPage = PageSelect.serectPage;

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

            if (nowSelect < minSelectStage)
            {
                nowSelect = maxSelectStage - 1;
            }

            //�{�^�����G��ꂽ�̂ŃN�[���^�C��������
            noTouchTime = 0.0f;
        }
        SelectActive(nowPage, nowSelect);
    }

    //�Z���N�g����Ă���{�^����Active�ɂ��鏈��
    private void SelectActive(int _nowPage,int _nowSelect)
    {
        stageArray[_nowPage, _nowSelect].transform.GetChild(0).GetComponent<Animator>().SetBool("bCheck", true);
        stageArray[_nowPage, _nowSelect].transform.GetChild(0).gameObject.SetActive(true);
    }
}
