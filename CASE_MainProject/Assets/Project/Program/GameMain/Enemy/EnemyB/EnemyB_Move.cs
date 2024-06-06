using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 2�_�Ԃ�⊮���Ĉړ�
public class EnemyB_Move : EnemyState
{
    [SerializeField, Header("�ړ��n�_�̐e�I�u�W�F�N�g")]
    GameObject targetParent;
    [SerializeField, Header("�ړ��n�_")]
    List<GameObject> targets;
    [SerializeField, Header("��]���x")]
    float angleSpeed;
    [SerializeField, Header("��]�����ύX�Ԋu")]
    float angleChangeDuration;
    bool bAngleReturn;
    float subCnt;
    [SerializeField, Header("�ړ����x")]
    float moveSpeed;
    [SerializeField, Header("�ړ�����"), ReadOnly]
    float moveTime;
    int targetIndex;
    bool bReturn = false;
    Vector3 moveStartPosition;
    GameObject rollBody;

    [Space(pad), Header("�J�ڐ惊�X�g")]
    [SerializeField, Header("���G�������̑J��")]
    public int searchSuccessID;
    [SerializeField, Header("�ړ�������̑J��")]
    public int completionID;
    [SerializeField, Header("��e���̑J��")]
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
