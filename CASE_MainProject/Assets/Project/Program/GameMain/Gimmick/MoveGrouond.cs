using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveGrouond : MonoBehaviour
{
    [SerializeField, Header("移動床オブジェクト")]
    public GameObject startPoint;

    [SerializeField, Header("移動床終着地点")]
    public GameObject endPoint;

    [SerializeField, Header("移動時間　※何秒で終着地点へ行くか")]
    float moveTime = 1.0f;

   [SerializeField, Header("到着後の停止時間　※到着後何秒後に戻り始めるか")]
    float stopTime = 0.0f;

    [SerializeField, Header("作動するまでの時間")]
    float delayTime = 3.0f;

    [SerializeField, Header("作動するまでのタイマー")]
    float delayTimer = 0.0f;

    [SerializeField, Header("自分のSteamScale")]
    Steam_Scale mySteam_Scale;

    [SerializeField, ReadOnly]
    Vector3 groundStartPosition;

    [SerializeField, ReadOnly]
    Quaternion groundStartQuaternion;

    [SerializeField, ReadOnly]
    float moveTimer = 0.0f;

    [SerializeField, Header("回転線形補間有効")]
    bool bRotationLerp = false;


    [SerializeField, Header("移動音オブジェクト")]
    GameObject moveSoundObject;

    [SerializeField, Header("停止音オブジェクト")]
    AudioSource au_StopSound;

    private void Awake()
    {
        //デバッグ用終着地点を非表示に
        endPoint.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        groundStartPosition = startPoint.transform.position;
        groundStartQuaternion = startPoint.transform.localRotation;

        mySteam_Scale.steam_Limit = moveTime + stopTime + delayTime + delayTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (mySteam_Scale.state == Steam_Scale.State.End)
        {
            if (delayTimer > 0.0f)
            {
                delayTimer -= Time.deltaTime;
            }
            else
            {
                moveTimer += Time.deltaTime;
            }

            moveSoundObject.SetActive(true);

            if (moveTimer > moveTime)
            {
                moveTimer = moveTime;

                delayTimer = delayTime;

                moveSoundObject.SetActive(false);
            }
        }

        if (mySteam_Scale.state == Steam_Scale.State.WaitStart)
        {
            if (delayTimer > 0.0f)
            {
                delayTimer -= Time.deltaTime;
            }
            else
            {
                moveTimer -= Time.deltaTime;
            }

            moveSoundObject.SetActive(true);

            if (moveTimer < 0.0f)
            {
                moveTimer = 0.0f;

                delayTimer = delayTime;

                moveSoundObject.SetActive(false);
            }
        }

        //座標の線形補間
        startPoint.transform.position = Vector3.Lerp(groundStartPosition, endPoint.transform.position, moveTimer / moveTime);

        if (bRotationLerp)
        {
            //回転の線形補間
            startPoint.transform.localRotation = Quaternion.Lerp(groundStartQuaternion, endPoint.transform.localRotation, moveTimer / moveTime);
        }
    }
}