using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class General_DeathSwitch : EnemyState
{
    [SerializeField, Header("スイッチ")]
    Switch switchObject;

    [SerializeField, Header("死亡時に渡す値")]
    bool provideValue = true;

    public override void Initialize()
    {
        switchObject = enemy.GetComponent<Switch>();
    }
    public override void Enter()
    {
        switchObject.SendMsg<bool>(0, provideValue);
        enemy.EnemyRigidbody.velocity = Vector3.zero;
        base.Enter();
        Destroy(enemy.gameObject);
        Destroy(gameObject);
    }
}
