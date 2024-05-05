using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class GetImagesInCanvas : MonoBehaviour
{
    [Header("画面の状態　0:再開/1:リスタート/2:セレクト")]
    [SerializeField] int selectButton;

    [Header("表示ボタンイラスト選択")]
    [Header("再開ボタン　ON/OFF")]
    [SerializeField]
    GameObject SaikaiObjectToActivate;
    [SerializeField] 
    GameObject SaikaiObjectToDeactivate;

    [Header("リスタート　ON/OFF")]
    [SerializeField]
    GameObject ReStartObjectToActivate;
    [SerializeField]
    GameObject ReStartObjectToDeactivate;

    [Header("セレクトボタン　ON/OFF")]
    [SerializeField]
    GameObject SelectObjectToActivate;
    [SerializeField]
    GameObject SelectObjectToDeactivate;


    void Start()
    {
        selectButton = 0;

        // SaikaiObjectToActivateを非アクティブにする
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
            // SaikaiObjectToActivateをアクティブにする
            SaikaiObjectToActivate.SetActive(true);
            // SaikaiObjectToDeactivateを非アクティブにする
            SaikaiObjectToDeactivate.SetActive(false);
        }
        else
        {
            // SaikaiObjectToActivateを非アクティブにする
            SaikaiObjectToActivate.SetActive(false);
            // SaikaiObjectToDeactivateをアクティブにする
            SaikaiObjectToDeactivate.SetActive(true);
        }

        if (selectButton == 1)
        {
            // ReStartObjectToActivateをアクティブにする
            ReStartObjectToActivate.SetActive(true);
            // ReStartObjectToDeactivateを非アクティブにする
            ReStartObjectToDeactivate.SetActive(false);
        }
        else
        {
            // ReStartObjectToActivateを非アクティブにする
            ReStartObjectToActivate.SetActive(false);
            // ReStartObjectToDeactivateをアクティブにする
            ReStartObjectToDeactivate.SetActive(true);
        }

        if (selectButton == 2)
        {
            // SelectObjectToActivateをアクティブにする
            SelectObjectToActivate.SetActive(true);
            // SelectObjectToDeactivateを非アクティブにする
            SelectObjectToDeactivate.SetActive(false);
        }
        else
        {
            // SelectObjectToActivateを非アクティブにする
            SelectObjectToActivate.SetActive(false);
            // SelectObjectToDeactivateをアクティブにする
            SelectObjectToDeactivate.SetActive(true);
        }
    }
}
