using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obj_move : MonoBehaviour
{
    [SerializeField, Header("ぶっ飛び速度")]
    float Force;


    static GameObject player;  //プレイヤー
    static Rigidbody rb;  //プレイヤーのRigidbody
    static GameObject Valve; //親の親のValve

    Vector3 change_Pos=new Vector3 (0.0f,1.0f,0.0f);

    // Start is called before the first frame update
    void Start()
    {
        Valve = transform.parent.parent.gameObject;
        player = GameObject.Find("Player");
        rb =player.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {




    }

    private void OnTriggerStay(Collider other)
    {
        //change_Pos = player.transform.position;

        if (other.gameObject == player)
        {
            rb.velocity = change_Pos;
        }

    }

}
