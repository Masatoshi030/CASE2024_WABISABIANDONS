using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveGrouond : MonoBehaviour
{
    [SerializeField, Header("�ړ����I�u�W�F�N�g")]
    GameObject moveGround;

    [SerializeField, Header("�ړ����I���n�_")]
    GameObject endPoint;

    [SerializeField, Header("�ړ����ԁ@�����b�ŏI���n�_�֍s����")]
    float moveTime = 1.0f;

    [SerializeField, Header("������SteamScale")]
    Steam_Scale mySteam_Scale;

    [SerializeField, ReadOnly]
    Vector3 groundStartPoint;

    [SerializeField, ReadOnly]
    float moveTimer = 0.0f;

    private void Awake()
    {
        //�f�o�b�O�p�I���n�_���\����
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
