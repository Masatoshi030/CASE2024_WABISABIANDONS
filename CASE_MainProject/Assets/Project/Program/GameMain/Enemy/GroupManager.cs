using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroupManager : Publisher
{
    [SerializeField, Header("エネミーへのアタッチか")]
    protected bool isAttachedEnemy = false;
}
