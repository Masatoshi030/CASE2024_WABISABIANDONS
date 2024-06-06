using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 2点間を補完して移動
public class EnemyB_Move : EnemyState
{
    [SerializeField, Header("移動地点の親オブジェクト")]
    GameObject targetParent;
    [SerializeField, Header("移動地点")]
    List<GameObject> targets;
    [SerializeField, Header("回転速度")]
    float angleSpeed;
    [SerializeField, Header("回転方向変更間隔")]
    float angleChangeDuration;
    bool bAngleReturn;
    float subCnt;
    [SerializeField, Header("移動速度")]
    float moveSpeed;
    [SerializeField, Header("移動時間"), ReadOnly]
    float moveTime;
    int targetIndex;
    bool bReturn = false;
    Vector3 moveStartPosition;
    GameObject rollBody;

    [Space(pad), Header("遷移先リスト")]
    [SerializeField, Header("索敵成功時の遷移")]
    public int searchSuccessID;
    [SerializeField, Header("移動完了後の遷移")]
    public int completionID;
    [SerializeField, Header("被弾時の遷移")]
    public int damagedID;

    public override void Initialize()
    {
        base.Initialize();
        rollBody = enemy.transform.Find("Body").gameObject;
        for(int i = 0; i < targetParent.transform.childCount;i++)
        {
            targets.Add(targetParent.transform.GetChild(i).gameObject);
        }
    }

    public override void Enter()
    {
        base.Enter();
        moveStartPosition = enemy.transform.position;
        float distance = Vector3.Distance(targets[targetIndex].transform.position, enemy.transform.position);
        moveTime = distance / moveSpeed;
        //enemy.transform.LookAt(targets[targetIndex].transform.position);
    }

    public override void MainFunc()
    {
        base.MainFunc();
        if(enemy.IsDamaged)
        {
            machine.TransitionTo(damagedID);
            return;
        }
        else if(enemy.IsFindPlayer)
        {
            machine.TransitionTo(searchSuccessID);
            return;
        }
        float value = machine.Cnt;
        value = value/moveTime > moveTime ? 1.0f : value/moveTime;
        enemy.transform.position = Vector3.Lerp(moveStartPosition, targets[targetIndex].transform.position, value);
        rollBody.transform.Rotate(Vector3.up, angleSpeed * Time.deltaTime);
        subCnt += Time.deltaTime;
        if (subCnt >= angleChangeDuration)
        {
            subCnt = 0.0f;
            bAngleReturn = !bAngleReturn;
        }

        if(value >= 1.0f)
        {
            machine.TransitionTo(completionID);
        }
    }

    public override void Exit()
    {
        base.Exit();
        
        if (!bReturn)
        {
            targetIndex++;
            if (targetIndex >= targets.Count - 1)
            {
                bReturn = true;
            }
        }
        else
        {
            targetIndex--;
            if(targetIndex == 0)
            {
                bReturn = false;
            }
        }
    }
}
