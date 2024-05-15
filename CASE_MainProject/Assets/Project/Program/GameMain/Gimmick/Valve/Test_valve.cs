using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Test_valve : MonoBehaviour
{
    static GameObject player;

    [SerializeField, Header("�����I�u�W�F�N�g")]
    GameObject move_Obj;

    [SerializeField,Header("�����̏ꏊ")]
    GameObject stop_Obj;

    [SerializeField, Header("�����X�s�[�h")]
    float move_speed;

    [SerializeField, Header("���쎞��")]
    float move_Timer;


    public float time;
    private Vector3 first_Obj;
    public bool on_Gimmick = false;
    public bool off_Gimmick = false;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        first_Obj = move_Obj.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
   
        //�M�~�b�N������
        if(on_Gimmick)
        {
            move_Gimmick();
        }

        //�M�~�b�N���߂�
        if(off_Gimmick)
        {
            remove_Gimmick();
        }

        //���[���I�������
        if (move_Obj.transform.position == stop_Obj.transform.position)
        {
            on_Gimmick = false;
            time += Time.deltaTime;
            if (move_Timer - time <= 0)
            {
                off_Gimmick = true;
                time = 0;
            }
        }




    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject==player)
        {
            //�쓮����
            if(!on_Gimmick)
            {
                this.on_Gimmick = true;
                this.off_Gimmick = false;
            }

            //�M�~�b�N���쓮�����㎞�Ԃ�L�΂�
            if (time>0)
            {
                time = 0;
            }


        }
    }

    //�M�~�b�N�����������ꍇ��
    void move_Gimmick()
    {
        //���ݒn�ƈړ��ꏊ�ƈړ��ʂňړ�
        move_Obj.transform.position = Vector3.MoveTowards(move_Obj.transform.position, stop_Obj.transform.position, move_speed*Time.deltaTime);
        Debug.Log("�쓮��");
    }

    void remove_Gimmick()
    {
        Debug.Log("�߂蒆");
        move_Obj.transform.position = Vector3.MoveTowards(move_Obj.transform.position, first_Obj, move_speed*Time.deltaTime);
    }
}
