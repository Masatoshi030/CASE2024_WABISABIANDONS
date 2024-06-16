using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrillComponent : MonoBehaviour
{

    [SerializeField, Header("攻撃力"), ReadOnly]
    float attackPower;
    public float AttackPower { get => attackPower; set => attackPower = value; }

    [SerializeField, Header("ライフタイム")]
    float lifeTime = 3.0f;
    public float LifeTime { get => lifeTime;set => lifeTime = value; }

    float lifCnt = 0.0f;

    private void Start()
    {
        lifCnt = 0.0f;
    }
    // Update is called once per frame
    void Update()
    {
        lifCnt += Time.deltaTime;

        if( lifCnt >= lifeTime)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Player")
        {
            PlayerController.instance.Damage(attackPower);
            Destroy(gameObject);
        }
        else if(collision.transform.tag == "Ground" || collision.transform.tag == "Wall")
        {
            Destroy(gameObject);
        }
    }
}
