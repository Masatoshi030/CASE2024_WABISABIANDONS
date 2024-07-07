using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldValve_Count : MonoBehaviour
{
    static public GoldValve_Count instance;

    [SerializeField, Header("ゴールドバルブタグ名")]
    string GoldValve="GoldValve";

    [SerializeField, Header("ゴールドバルブタグ名")]
    string Enemy = "Enemy";

    public int Get_Count;

    public GameObject[] GoldValve_C;
    public GameObject[] Enemy_C;

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
        GoldValve_C= GameObject.FindGameObjectsWithTag(GoldValve);
        Enemy_C=GameObject.FindGameObjectsWithTag(Enemy);  
    }

    
    void Update()
    {

    }

    public void AddValveCount()
    {
        Get_Count++;
    }

    public int SetValveCount()
    {
        return Get_Count;
    }

}
