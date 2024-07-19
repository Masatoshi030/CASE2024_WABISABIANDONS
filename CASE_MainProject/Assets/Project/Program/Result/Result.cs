using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Result : MonoBehaviour
{

    [SerializeField, Header("バルブの数")]
    public TextMeshProUGUI valve_Score;

    [SerializeField, Header("敵の数")]
    public TextMeshProUGUI enemy_Score;
    
    public int result_Valve;  //リザルトに出す数字
    public int result_Enemy;

    void Start()
    {
        //他のスクリプトから値を取得してくる
        result_Valve = PlayerPrefs.GetInt("[GoldValve]GetCount");
        result_Enemy = PlayerPrefs.GetInt("[Enemy]KillCount");

        valve_Score.text = result_Valve.ToString();
        enemy_Score.text = result_Enemy.ToString();

    }

    // Update is called once per frame
    void Update()
    {

    }
}
