using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DataManager : MonoBehaviour
{
    public static DataManager instance;

    private void Start()
    {
        if (instance == null)
        {
            // 自身をインスタンスとする
            instance = this;
        }
        else
        {
            // インスタンスが既に存在していたら自身を消去する
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        //セーブデーター消去
        if (Input.GetKeyDown(KeyCode.Delete))
        {
            PlayerPrefs.DeleteAll();
        }
    }

    public void SaveClearThisStage()
    {
        string stageDataName = "SELECT_CLEAR[" + SceneManager.GetActiveScene().name + "]";
        PlayerPrefs.SetInt(stageDataName, 1);

        Debug.Log(stageDataName + "のクリアデータをセーブしました");

        PlayerPrefs.SetInt("[GoldValve]GetCount", GoldValve_Count.instance.GetValveCount());
        PlayerPrefs.SetInt("[Enemy]KillCount", (int)Enemy_Manager.instance.GetDefeatEnemyNum());

        Debug.Log(GoldValve_Count.instance.GetValveCount() + "のゴールドバルブをカウントしました。");
        Debug.Log((int)Enemy_Manager.instance.GetDefeatEnemyNum() + "の敵のキルをカウントしました。");
    }
}
