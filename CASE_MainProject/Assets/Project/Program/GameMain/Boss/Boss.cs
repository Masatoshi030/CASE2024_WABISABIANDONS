using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.IO;

public class Boss : Enemy
{
    public static Boss instance;

    [SerializeField, Header("スタン時の非アクティブオブジェクト")]
    GameObject[] touchActivationObject;

    [SerializeField, Header("マネージャー")]
    Boss_Manager manager;
    public Boss_Manager Manager { get => manager; }

    BossStateMachine bossStateMachine;

    bool isFinishAnimation = false;
    public bool IsFinishAnimation { get => isFinishAnimation; set => isFinishAnimation = value; }

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        if (target == null)
        {
            target = GameObject.Find("Player");
        }
        eyeTransform = transform.Find("EyeTransform");
        rb = GetComponent<Rigidbody>();
        if (GetComponent<NavMeshAgent>())
            enemyAgent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        // パラメータの初期化
        enemyHp = maxHp;
        enemyPressure = maxPressure;
        // ステートマシンの設定
        bossStateMachine = GetComponent<BossStateMachine>();
        bossStateMachine.BossComponent = this;
        bossStateMachine.Initialize();
    }

    private void Update()
    {
        // 距離と角度を保存しておく
        (isFindPlayer, toPlayerDistance, toPlayerAngle) = FindPlayerAtFOV();
        toPlayerDistance = Mathf.Sqrt(toPlayerDistance);
        Debug.Log(toPlayerDistance);

        if(bossStateMachine.IsUpdate)
        {
            bossStateMachine.MainFunc();
        }
    }

    public override void ReceiveMsg<T>(Connection sender, int msgType, T msg)
    {
        if(msgType == 0)
        {
            isFinishAnimation = true;
        }
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        bossStateMachine.TriggerEnterSelf(other);
    }

    protected virtual void OnCollisionEnter(Collision collision)
    {
        bossStateMachine.CollisionEnterSelf(collision);
    }

    protected virtual void OnTriggerStay(Collider other)
    {
        bossStateMachine.TriggerStaySelf(other);
    }

    protected virtual void OnCollisionStay(Collision collision)
    {
        bossStateMachine.CollisionStaySelf(collision);
    }

    protected virtual void OnTriggerExit(Collider other)
    {
        bossStateMachine.TriggerExitSelf(other);
    }

    protected virtual void OnCollisionExit(Collision collision)
    {
        bossStateMachine.CollisionExitSelf(collision);
    }

    public void Stan()
    {
        for(int i = 0; i < touchActivationObject.Length; i++)
        {
            touchActivationObject[i].SetActive(false);
        }
    }

    public void Reboot()
    {
        for (int i = 0; i < touchActivationObject.Length; i++)
        {
            touchActivationObject[i].SetActive(true);
        }
    }

    public override bool Damage(float damage, Vector3 direction)
    {
        // ダメージの代わりに怒りに蓄積
        manager.TouchEmotion(Boss_Manager.Emotion.Anger, 3);

        return false;
    }
}
