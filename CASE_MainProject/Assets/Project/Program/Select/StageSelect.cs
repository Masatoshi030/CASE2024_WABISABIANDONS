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
        GameObject paper1 = GameObject.Find("Paper_1");
        if (paper1 != null )
        {
            //Paper_1�̎q�I�u�W�F�N�g���擾
            stageArray[0, 0] = paper1.transform.Find("Line_1_1/Select_1_1")?.gameObject;
            stageArray[0, 1] = paper1.transform.Find("Line_1_2/Select_1_2")?.gameObject;
            stageArray[0, 2] = paper1.transform.Find("Line_1_3/Select_1_3")?.gameObject;
            stageArray[0, 3] = paper1.transform.Find("Line_1_4/Select_1_4")?.gameObject;
            stageArray[0, 4] = paper1.transform.Find("Line_1_5/Select_1_5")?.gameObject;
            stageArray[0, 5] = paper1.transform.Find("Line_1_6/Select_1_6")?.gameObject;
        }

        // Paper_2�I�u�W�F�N�g���擾
        GameObject paper2 = GameObject.Find("Paper_2");
        if (paper2 != null)
        {
            // Paper_2�̎q�I�u�W�F�N�g��z��Ɋ��蓖��
            stageArray[1, 0] = paper2.transform.Find("Line_2_1/Select_2_1")?.gameObject;
            stageArray[1, 1] = paper2.transform.Find("Line_2_2/Select_2_2")?.gameObject;
            stageArray[1, 2] = paper2.transform.Find("Line_2_3/Select_2_3")?.gameObject;
            stageArray[1, 3] = paper2.transform.Find("Line_2_4/Select_2_4")?.gameObject;
            stageArray[1, 4] = paper2.transform.Find("Line_2_5/Select_2_5")?.gameObject;
            stageArray[1, 5] = paper2.transform.Find("Line_2_6/Select_2_6")?.gameObject;
        }

        // Paper_3�I�u�W�F�N�g���擾
        GameObject paper3 = GameObject.Find("Paper_3");
        if (paper3 != null)
        {
            // Paper_2�̎q�I�u�W�F�N�g��z��Ɋ��蓖��
            stageArray[2, 0] = paper3.transform.Find("Line_3_1/Select_3_1")?.gameObject;
            stageArray[2, 1] = paper3.transform.Find("Line_3_2/Select_3_2")?.gameObject;
            stageArray[2, 2] = paper3.transform.Find("Line_3_3/Select_3_3")?.gameObject;
            stageArray[2, 3] = paper3.transform.Find("Line_3_4/Select_3_4")?.gameObject;
            stageArray[2, 4] = paper3.transform.Find("Line_3_5/Select_3_5")?.gameObject;
            stageArray[2, 5] = paper3.transform.Find("Line_3_6/Select_3_6")?.gameObject;
        }

        // Paper_4�I�u�W�F�N�g���擾
        GameObject paper4 = GameObject.Find("Paper_4");
        if (paper4 != null)
        {
            // Paper_4�̎q�I�u�W�F�N�g��z��Ɋ��蓖��
            stageArray[3, 0] = paper4.transform.Find("Line_4_1/Select_4_1")?.gameObject;
            stageArray[3, 1] = paper4.transform.Find("Line_4_2/Select_4_2")?.gameObject;
            stageArray[3, 2] = paper4.transform.Find("Line_4_3/Select_4_3")?.gameObject;
            stageArray[3, 3] = paper4.transform.Find("Line_4_4/Select_4_4")?.gameObject;
            stageArray[3, 4] = paper4.transform.Find("Line_4_5/Select_4_5")?.gameObject;
            stageArray[3, 5] = paper4.transform.Find("Line_4_6/Select_4_6")?.gameObject;
        }


        //for (int i = minPage; i < maxPage; i++)
        //{
        //    for (int j = minSelectStage; j < maxSelectStage; j++)
        //    {
        //        if (stageArray[i, j] != null)
        //        {
        //            Debug.Log(stageArray[i, j].name);
        //            stageArray[i, j].SetActive(false);
        //        }
        //    }
        //}
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
                    stageArray[i, j].SetActive(false);
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
        stageArray[_nowPage, _nowSelect].SetActive(true);
    }
}
