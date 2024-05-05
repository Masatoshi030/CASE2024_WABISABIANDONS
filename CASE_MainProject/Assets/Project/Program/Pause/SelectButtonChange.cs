using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class GetImagesInCanvas : MonoBehaviour
{
    [Header("��ʂ̏�ԁ@0:�ĊJ/1:���X�^�[�g/2:�Z���N�g")]
    [SerializeField] int selectButton;

    [Header("�\���{�^���C���X�g�I��")]
    [Header("�ĊJ�{�^���@ON/OFF")]
    [SerializeField]
    GameObject SaikaiObjectToActivate;
    [SerializeField] 
    GameObject SaikaiObjectToDeactivate;

    [Header("���X�^�[�g�@ON/OFF")]
    [SerializeField]
    GameObject ReStartObjectToActivate;
    [SerializeField]
    GameObject ReStartObjectToDeactivate;

    [Header("�Z���N�g�{�^���@ON/OFF")]
    [SerializeField]
    GameObject SelectObjectToActivate;
    [SerializeField]
    GameObject SelectObjectToDeactivate;


    void Start()
    {
        selectButton = 0;

        // SaikaiObjectToActivate���A�N�e�B�u�ɂ���
        SaikaiObjectToActivate.SetActive(false);
        ReStartObjectToActivate.SetActive(false);
        SelectObjectToActivate.SetActive(false);
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.DownArrow))
        {
            selectButton++;
            if (selectButton > 2)
            {
                selectButton = 0;
            }
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            selectButton--;

            if(selectButton<0)
            {
                selectButton = 2;
            }
        }

        if(selectButton ==0)
        {
            // SaikaiObjectToActivate���A�N�e�B�u�ɂ���
            SaikaiObjectToActivate.SetActive(true);
            // SaikaiObjectToDeactivate���A�N�e�B�u�ɂ���
            SaikaiObjectToDeactivate.SetActive(false);
        }
        else
        {
            // SaikaiObjectToActivate���A�N�e�B�u�ɂ���
            SaikaiObjectToActivate.SetActive(false);
            // SaikaiObjectToDeactivate���A�N�e�B�u�ɂ���
            SaikaiObjectToDeactivate.SetActive(true);
        }

        if (selectButton == 1)
        {
            // ReStartObjectToActivate���A�N�e�B�u�ɂ���
            ReStartObjectToActivate.SetActive(true);
            // ReStartObjectToDeactivate���A�N�e�B�u�ɂ���
            ReStartObjectToDeactivate.SetActive(false);
        }
        else
        {
            // ReStartObjectToActivate���A�N�e�B�u�ɂ���
            ReStartObjectToActivate.SetActive(false);
            // ReStartObjectToDeactivate���A�N�e�B�u�ɂ���
            ReStartObjectToDeactivate.SetActive(true);
        }

        if (selectButton == 2)
        {
            // SelectObjectToActivate���A�N�e�B�u�ɂ���
            SelectObjectToActivate.SetActive(true);
            // SelectObjectToDeactivate���A�N�e�B�u�ɂ���
            SelectObjectToDeactivate.SetActive(false);
        }
        else
        {
            // SelectObjectToActivate���A�N�e�B�u�ɂ���
            SelectObjectToActivate.SetActive(false);
            // SelectObjectToDeactivate���A�N�e�B�u�ɂ���
            SelectObjectToDeactivate.SetActive(true);
        }
    }
}
