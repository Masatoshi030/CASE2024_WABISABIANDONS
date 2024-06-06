using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class General_DeathSwitch : EnemyState
{
    [SerializeField, Header("�X�C�b�`")]
    Switch switchObject;

    [SerializeField, Header("���S���ɓn���l")]
    bool provideValue = true;

    public override void Initialize()
    {
        base.Initialize();

        switchObject = enemy.GetComponent<Switch>();
    }
    public override void Enter()
    {
        base.Enter();

        switchObject.SendMsg<bool>(0, provideValue);
        enemy.EnemyRigidbody.velocity = Vector3.zero;
        Destroy(enemy.gameObject);
        Destroy(gameObject);
    }
}
