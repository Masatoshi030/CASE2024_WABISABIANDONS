using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Result : MonoBehaviour
{

    [SerializeField, Header("�o���u�̐�")]
    GameObject Valve_Moji;

    [SerializeField, Header("�G�̐�")]
    GameObject Enemy_Moji;

    public int result_Valve;  //���U���g�ɏo������
    public uint result_Enemy;

    void Start()
    {
        //���̃X�N���v�g����l���擾���Ă���
        result_Valve = GoldValve_Count.instance.SetValveCount();
        result_Enemy = Enemy_Manager.instance.GetDefeatEnemyNum();

        TextMeshPro valve_Score=Valve_Moji.GetComponent<TextMeshPro>();
        TextMeshPro enemy_Score = Enemy_Moji.GetComponent<TextMeshPro>();

        valve_Score.text = result_Valve.ToString();
        enemy_Score.text = result_Enemy.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
