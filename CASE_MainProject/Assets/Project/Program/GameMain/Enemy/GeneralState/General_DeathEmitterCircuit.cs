using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �M�~�b�N���쓮���������^
public class General_DeathEmitterCircuit : EnemyState
{
    [SerializeField, Header("���S���ɐ�������G�t�F�N�g")]
    GameObject spawnObj;
    [SerializeField, Header("�G�t�F�N�g�̈ړ���")]
    Transform targetTransform;
    [SerializeField, Header("�G�t�F�N�g�̈ړ��ɂ����鎞��")]
    float effectMoveTime = 1.0f;
    [SerializeField, Header("�A�g���H")]
    List<Circuit_ActiveOperation> list;
    [SerializeField, Header("��H�ɓn���l")]
    bool present = true;

    public override void Enter()
    {
        base.Enter();

        GameObject obj = Instantiate(spawnObj, transform.position, Quaternion.identity);
        EffectMoveConnectCircuit com = obj.GetComponent<EffectMoveConnectCircuit>();
        com.Excute(enemy.transform.position, targetTransform.position, effectMoveTime);
        com.Circuits = list;
        com.Present = present;

        enemy.EnemyRigidbody.velocity = Vector3.zero;
        Destroy(enemy.gameObject);
        Destroy(gameObject);
    }
}
