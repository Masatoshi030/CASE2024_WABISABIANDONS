using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Text_randomroulette : MonoBehaviour
{
    public enum Text_choice
    {
        Valve,
        Enemy,
    }

    public TextMeshProUGUI textMeshPro;

    [SerializeField, Header("動かす数字"), Toolbar(typeof(Text_choice))]
    public Text_choice text_Move = Text_choice.Valve;

    [SerializeField,Header("Animationを流す時間")]
    public float randomTimer=20;

   public int finalNumber= 12;

    private bool isDisplayingRandomNumbers = true;

    // Start is called before the first frame update
    void Start()
    {
        //switch(text_Move)
        //{
        //    case Text_choice.Valve:
        //        finalNumber = GoldValve_Count.instance.SetValveCount();
        //        break;

        //        case Text_choice.Enemy:
        //        finalNumber =(int)Enemy_Manager.instance.GetDefeatEnemyNum();
        //        break;
        //}

        // ランダムな数字を表示するコルーチンを開始
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
            textMeshPro.text = randomNumber.ToString();

            // フレームを待機
            yield return new WaitForSeconds(0.001f); // 0.1秒ごとに更新

            elapsedTime += 0.1f;
        }

        textMeshPro.text=finalNumber.ToString();
    }
}

