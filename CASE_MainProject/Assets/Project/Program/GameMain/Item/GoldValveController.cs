using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GoldValveController : MonoBehaviour
{

    [SerializeField, Header("プレイヤーに向かうスピード")]
    float speed = 2.0f;

    float lerpValue = 0.0f;

    [SerializeField, Header("取得フラグ")]
    bool bGetFlag = false;

    public Vector3 startPosition;
    private void Start()
    {
        startPosition = transform.position;
    }

    public void GetGoldValve()
    {
        bGetFlag = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (bGetFlag)
        {
            //線形補間の現在の位置をカウント
            lerpValue += Time.deltaTime * speed;

            //現在の位置カウントから座標を計算
            transform.position = Vector3.Lerp(startPosition, PlayerController.instance.transform.position, lerpValue);

            //もしプレイヤーに到着したら削除
            if(lerpValue > 1.0f)
            {
                Destroy(this.gameObject);
            }
        }
    }
}
