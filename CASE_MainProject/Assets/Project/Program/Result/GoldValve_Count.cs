using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldValve_Count : MonoBehaviour
{
    static public GoldValve_Count instance;//一つのシーンに一つだけ存在するもの

    [SerializeField, Header("ゴールドバルブタグ名")]
    string targetValveTag = "GoldValve";

    [SerializeField, Header("敵のタグ名")]
    string targetEnemyTag = "Enemy";

    [SerializeField, Header("オブジェクトのタグ名")]
    string targetObjectTag = "BreakBox";

    [SerializeField,Header("獲得したゴールドバルブの数"),ReadOnly]
    public int getValveCount;

    public GameObject[] goldValves;
    public GameObject[] enemies;
    public GameObject[] objects;

    [SerializeField, Header("配置バルブの数"), ReadOnly]
    private int maxStageValveNum;
    [SerializeField, Header("敵から取得できる最大数"), ReadOnly] // ビリヤードの巻き込みを考慮しない
    private int maxEnemyValveNum;
    [SerializeField, Header("オブジェクトから取得できる最大数"), ReadOnly]
    private int maxObjectValveNum;

    [SerializeField, Header("ステージから取得できる最大数"), ReadOnly]
    public int totlaGoldValveNum;

    //Startの前に処理をする
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
    }

    void Start()
    {
        goldValves = GameObject.FindGameObjectsWithTag(targetValveTag);
        maxStageValveNum = goldValves.Length;
        enemies = GameObject.FindGameObjectsWithTag(targetEnemyTag);
        for(int i = 0; i < enemies.Length; i++)
        {
            if(enemies[i].GetComponent<Enemy>() != null)
            {
                Enemy enemy = enemies[i].GetComponent<Enemy>();
                maxEnemyValveNum += enemy.DropValveNum;
            }
        }
        objects = GameObject.FindGameObjectsWithTag(targetObjectTag);
        for (int i = 0; i < objects.Length; i++)
        {
            if (objects[i].GetComponent<GoldValveDropper>() != null)
            {
                GoldValveDropper dropper = objects[i].GetComponent<GoldValveDropper>();
                maxObjectValveNum += dropper.DropValveNum;
            }
        }
        totlaGoldValveNum = maxEnemyValveNum + maxStageValveNum + maxObjectValveNum;
    }

    
    void Update()
    {

    }

    public void AddValveCount()
    {
        getValveCount++;
    }

    public int GetValveCount()
    {
        return getValveCount;
    }

}
