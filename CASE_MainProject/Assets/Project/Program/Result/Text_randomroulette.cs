using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Text_randomroulette : MonoBehaviour
{
    public Animator result_Anim;

    public TextMeshProUGUI textMeshPro_Valve;
    public TextMeshProUGUI textMeshPro_Enemy;

    [SerializeField, Header("Animationを流す時間")]
    public float randomTimer = 20;

    [SerializeField, Header("数字の更新時間")]
    public float update_nunber=0.01f;

    //最後に表示する数字
    public int finalNumber_Valve = 0;
    public int finalNumber_Enemy = 0;

    private bool finish_Rondom=false;
   
    // Start is called before the first frame update
    void Start()
    {
        //倒した敵と獲得したバルブの数を取得していく
        finalNumber_Valve = GoldValve_Count.instance.SetValveCount();
        finalNumber_Enemy = (int)Enemy_Manager.instance.GetDefeatEnemyNum();

        // ランダムな数字を表示する関数
        StartCoroutine(DisplayRandomNumbers());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator DisplayRandomNumbers()
    {
        float elapsedTime = 0f;

        while (elapsedTime < randomTimer)
        {
            // ランダムな数字を生成して表示
            int randomNumber = Random.Range(0, 1000); // 0から99のランダムな数字
            textMeshPro_Valve.text = randomNumber.ToString();
            randomNumber = Random.Range(0, 1000); // 0から99のランダムな数字
            textMeshPro_Enemy.text = randomNumber.ToString();

            // フレームを待機
            yield return new WaitForSeconds(update_nunber); // 更新

            elapsedTime += 1.0f;
        }

        textMeshPro_Valve.text=finalNumber_Valve.ToString();
        textMeshPro_Enemy.text=finalNumber_Enemy.ToString();

        result_Anim.SetBool("Start_Anim", true) ;
    }
}

