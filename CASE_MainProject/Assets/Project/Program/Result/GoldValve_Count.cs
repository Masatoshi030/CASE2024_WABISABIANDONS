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

    [SerializeField,Header("獲得したゴールドバルブの数"),ReadOnly]
    public int getValveCount;

    public GameObject[] goldValves;
    public GameObject[] enemies;

    [SerializeField, Header("配置バルブの数"), ReadOnly]
    private int maxStageValveNum;
    [SerializeField, Header("敵から取得最大数"), ReadOnly] // ビリヤードの巻き込みを考慮しない
    private int maxEnemyValveNum;
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
        totlaGoldValveNum = maxEnemyValveNum + maxStageValveNum;
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
