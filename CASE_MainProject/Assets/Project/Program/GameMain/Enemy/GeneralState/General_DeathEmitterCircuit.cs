using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ギミックを作動させる特殊型
public class General_DeathEmitterCircuit : EnemyState
{
    [SerializeField, Header("死亡時に生成するエフェクト")]
    GameObject spawnObj;
    [SerializeField, Header("エフェクトの移動先")]
    Transform targetTransform;
    [SerializeField, Header("エフェクトの移動にかける時間")]
    float effectMoveTime = 1.0f;
    [SerializeField, Header("連携先回路")]
    List<Circuit_ActiveOperation> list;
    [SerializeField, Header("回路に渡す値")]
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
