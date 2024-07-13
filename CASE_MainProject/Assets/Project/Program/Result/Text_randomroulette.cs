using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Text_randomroulette : MonoBehaviour
{
    [SerializeField, Header("リザルトで使用してるAnimator")]
    public Animator result_Anim;

    [SerializeField, Header("ゴールドバルブ取得の数を示すテキスト")]
    public TextMeshProUGUI textMeshPro_Valve;

    [SerializeField, Header("倒した敵の数を示すテキスト")]
    public TextMeshProUGUI textMeshPro_Enemy;

    [SerializeField, Header("Animationを流す時間")]
    public float randomTimer = 20;

    [SerializeField, Header("数字の更新時間")]
    public float update_nunber=0.01f;

    [SerializeField, Header("遷移するSecen")]
    string stageName;

    [SerializeField, Header("×ボタンのクールタイム")]
    float buttonCoolTime=0.25f;
    [SerializeField, Header("×ボタンを触っていない時間"),ReadOnly]
    float noTouchTime;

    //最後に表示する数字
    private int finalNumber_Valve = 0;
    private int finalNumber_Enemy = 0;

    private bool finish_Rondom=true;  //ランダムが終わったか
   
    // Start is called before the first frame update
    void Start()
    {
        //倒した敵と獲得したバルブの数を取得していく
        finalNumber_Valve = GoldValve_Count.instance.SetValveCount();
        finalNumber_Enemy = (int)Enemy_Manager.instance.GetDefeatEnemyNum();


    }

    // Update is called once per frame
    void Update()
    {
        noTouchTime += Time.deltaTime;

        // ランダムな数字を表示する関数
        StartCoroutine(DisplayRandomNumbers());

        //ステージ遷移
        if(noTouchTime> buttonCoolTime) 
        {
            Next_Stage();
        }
       

    }

    //ランダムの数字を出す
    IEnumerator DisplayRandomNumbers()
    {
        if(finish_Rondom)
        {
            float elapsedTime = 0f;

            while (elapsedTime < randomTimer&&finish_Rondom)
            {
                // ランダムな数字を生成して表示
                int randomNumber = Random.Range(0, 1000); // 0から99のランダムな数字
                textMeshPro_Valve.text = randomNumber.ToString();
                randomNumber = Random.Range(0, 1000); // 0から99のランダムな数字
                textMeshPro_Enemy.text = randomNumber.ToString();

                // フレームを待機
                yield return new WaitForSeconds(update_nunber); // 更新

                elapsedTime += 1.0f;

                if (DualSense_Manager.instance.GetInputState().CrossButton == DualSenseUnity.ButtonState.NewDown)
                {
                    noTouchTime = 0.0f;
                    finish_Rondom = false;
                    result_Anim.SetTrigger("Stamp_finish");
                }


            }

            finish_Rondom = false;

            textMeshPro_Valve.text = finalNumber_Valve.ToString();
            textMeshPro_Enemy.text = finalNumber_Enemy.ToString();

            result_Anim.SetBool("Start_Anim", true);
        }
    }

    void Next_Stage()
    {
        if (DualSense_Manager.instance.GetInputState().CrossButton == DualSenseUnity.ButtonState.NewDown)
        { 
            this.GetComponent<SceneChanger>().SceneChange(stageName);
        }
    }

}

    