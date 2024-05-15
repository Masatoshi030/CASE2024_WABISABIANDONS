using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Test_valve : MonoBehaviour
{
    static GameObject player;

    [SerializeField, Header("動くオブジェクト")]
    GameObject move_Obj;

    [SerializeField,Header("動作後の場所")]
    GameObject stop_Obj;

    [SerializeField, Header("動くスピード")]
    float move_speed;

    [SerializeField, Header("動作時間")]
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
   
        //ギミックが発動
        if(on_Gimmick)
        {
            move_Gimmick();
        }

        //ギミックが戻る
        if(off_Gimmick)
        {
            remove_Gimmick();
        }

        //収納が終わった時
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
            //作動する
            if(!on_Gimmick)
            {
                this.on_Gimmick = true;
                this.off_Gimmick = false;
            }

            //ギミックが作動した後時間を伸ばす
            if (time>0)
            {
                time = 0;
            }


        }
    }

    //ギミックが発動した場合の
    void move_Gimmick()
    {
        //現在地と移動場所と移動量で移動
        move_Obj.transform.position = Vector3.MoveTowards(move_Obj.transform.position, stop_Obj.transform.position, move_speed*Time.deltaTime);
        Debug.Log("作動中");
    }

    void remove_Gimmick()
    {
        Debug.Log("戻り中");
        move_Obj.transform.position = Vector3.MoveTowards(move_Obj.transform.position, first_Obj, move_speed*Time.deltaTime);
    }
}
