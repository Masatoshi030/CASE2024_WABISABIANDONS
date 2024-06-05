using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyC : Enemy
{
    static protected GameObject cameraObject;

    public enum PressureState
    { 
        [InspectorName("空")]Empty, 
        [InspectorName("普通")]Medium,
        [InspectorName("パンパン")]Full
    }

    [SerializeField, Header("圧力状態"), Toolbar(typeof(PressureState))]
    protected PressureState pressureState = PressureState.Empty;
    public PressureState CState { get => pressureState; set => pressureState = value;}

    // 受けたダメージの保存
    float damageValue;
    public float DamageValue { get => damageValue; }


    private void Awake()
    {
        base.Awake();
        if (cameraObject == null)
            cameraObject = GameObject.Find("PlayerCamera_Brain");
    }

    protected void Start()
    {
        base.Start();
    }

    protected void Update()
    {
        if(isSearchPlayer)
        {
            (bool find, float dis) = FindPlayerAtFOV();
            isFindPlayer = find;
            toPlayerDistance = dis;
        }
        enemyStateMachine.MainFunc();
    }

    public override bool Damage(float damage, Vector3 direction)
    {
        // プレイヤーとのポジションの差分で方向を保存するパターン

        // シンプルにポジションの差を計算
        Transform tra = PlayerController.instance.transform;
        Vector3 dir = transform.position - tra.position;
        damageVector = dir;

        //// 衝突点の検知
        //Ray ray = new Ray(tra.position, tra.forward);
        //RaycastHit[] hits = Physics.RaycastAll(ray, 10.0f);
        //for(int i = 0; i < hits.Length;i++)
        //{
        //    if (hits[i].collider.gameObject == gameObject)
        //    {
        //        Debug.Log("player");
        //        Vector3 hitPoint = hits[i].point;
        //        damageVector = transform.position - hitPoint;
        //        damageVector.y = 0.0f;
        //        break;
        //    }
        //    if (i== hits.Length - 1)
        //    {
        //        damageVector = dir;
        //    }
        //}

        // カメラの向きで方向を保存するパターン
        //damageVector = cameraObject.transform.forward;
        //damageVector.y = 0.0f;
        damageVector.Normalize();
        damageValue = damage;
        isDamaged = true;
        return false;
    }
}
