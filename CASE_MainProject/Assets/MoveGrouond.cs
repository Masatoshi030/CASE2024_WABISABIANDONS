using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveGrouond : MonoBehaviour
{
    [SerializeField, Header("移動床オブジェクト")]
    GameObject moveGround;

    [SerializeField, Header("移動床終着地点")]
    GameObject endPoint;

    [SerializeField, Header("移動時間　※何秒で終着地点へ行くか")]
    float moveTime = 1.0f;

    [SerializeField, Header("自分のSteamScale")]
    Steam_Scale mySteam_Scale;

    [SerializeField, ReadOnly]
    Vector3 groundStartPoint;

    [SerializeField, ReadOnly]
    float moveTimer = 0.0f;

    private void Awake()
    {
        //デバッグ用終着地点を非表示に
        endPoint.GetComponent<MeshRenderer>().enabled = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        groundStartPoint = moveGround.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (mySteam_Scale.state == Steam_Scale.State.End)
        {
            moveTimer += Time.deltaTime;
            if(moveTimer > moveTime)
            {
                moveTimer = moveTime;
            }
        }

        if (mySteam_Scale.state == Steam_Scale.State.WaitStart)
        {
            moveTimer -= Time.deltaTime;
            if (moveTimer < 0.0f)
            {
                moveTimer = 0.0f;
            }
        }

        moveGround.transform.position = Vector3.Lerp(groundStartPoint, endPoint.transform.position, moveTimer / moveTime);
    }
}
