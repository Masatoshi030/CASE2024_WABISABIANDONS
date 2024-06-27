using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_B_Move : EnemyState
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
    float remainingDistance = 0.0f;
    Vector3 moveStartPosition;
    Vector3 moveDirection;

    [Space(pad), Header("--�J�ڐ惊�X�g--")]
    [SerializeField, Header("�������̑J��")]
    StateKey findingKey;
    [SerializeField, Header("�������̑J��")]
    StateKey completionKey;
    [SerializeField, Header("�_���[�W���̑J��")]
    StateKey damagedKey;



    public override void Initialize()
    {
        base.Initialize();
        for (int i = 0; i < targetParent.transform.childCount; i++)
        {
            targets.Add(targetParent.transform.GetChild(i).gameObject);
        }
    }

    public override void Enter()
    {
        base.Enter();
        moveStartPosition = enemy.transform.position;
        remainingDistance = Vector3.Distance(targets[targetIndex].transform.position, enemy.transform.position);

        moveDirection = (targets[targetIndex].transform.position - enemy.transform.position).normalized;
    }

    public override void MainFunc()
    {
        base.MainFunc();
        if (enemy.IsDamaged)
        {
            machine.TransitionTo(damagedKey);
            return;
        }
        else if (enemy.IsFindPlayer)
        {
            machine.TransitionTo(findingKey);
            enemy.SendMsg<int>(0, 0);
            return;
        }
        // ����̃t���[���̈ړ���
        float currentFrameMoveAmout = moveSpeed * Time.deltaTime;
        bool isCompletion = false;
        remainingDistance -= currentFrameMoveAmout;
        if(remainingDistance <= 0.0f)
        {
            currentFrameMoveAmout -= remainingDistance;
            isCompletion = true;
        }
        enemy.transform.Translate(moveDirection * currentFrameMoveAmout);


        if (subCnt >= angleChangeDuration)
        {
            subCnt = 0.0f;
            bAngleReturn = !bAngleReturn;
        }
        if (isCompletion)
        {
            machine.TransitionTo(completionKey);
        }
    }

    public override void Exit()
    {
        base.Exit();
        subCnt = 0.0f;

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
            if (targetIndex == 0)
            {
                bReturn = false;
            }
        }
    }
}
