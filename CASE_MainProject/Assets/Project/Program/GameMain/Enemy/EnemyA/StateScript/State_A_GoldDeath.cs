using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_A_GoldDeath : EnemyState
{

    [SerializeField, Header("îrèoâÒêî")]
    int repeatNum;
    int repeatCnt;

    [SerializeField, Header("ä‘äu")]
    float duration;
    float subCnt = 0.0f;

    AudioSource source;

    public override void Initialize()
    {
        base.Initialize();

        source = enemy.GetComponent<AudioSource>();
    }

    public override void Enter()
    {
        base.Enter();
        repeatCnt = 1;
        enemy.EnemyCollider.enabled = false;
        enemy.IsVelocityZero = true;

        // 1âÒñ⁄ÇÃîrèo
        if (enemy.IsDropValves)
        {
            DropValveManager.instance.CreateValvesCustom(enemy.DropValveNum, enemy.transform.position, enemy.IsAutoGet, 1.0f, 10.0f);
            source.PlayOneShot(source.clip);
        }
    }
    public override void MainFunc()
    {
        base.MainFunc();
        subCnt += Time.deltaTime;

        if(repeatCnt == repeatNum)
        {
            if (enemy.countEnable)
                Enemy_Manager.instance.AddDefeatEnemy();

            Enemy_Manager.instance.CreateCollisionEffect(enemy.transform.position, Quaternion.identity);
            enemy.DestroyAllow();
            Destroy(enemy.gameObject);
            Destroy(gameObject);
        }
        else if(subCnt >= duration)
        {
            repeatCnt++;
            if (enemy.IsDropValves)
            {
                DropValveManager.instance.CreateValvesCustom(enemy.DropValveNum, enemy.transform.position, enemy.IsAutoGet, 1.0f, 10.0f);
                source.PlayOneShot(source.clip);
            }
            subCnt = 0.0f;
        }
    }
}