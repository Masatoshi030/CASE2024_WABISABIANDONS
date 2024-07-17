using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class GoalManager : MonoBehaviour
{
    public static GoalManager instance;

    [SerializeField, Header("ゴールのバーチャルカメラ")]
    CinemachineVirtualCamera goalVirtualCamera;

    private void Start()
    {
        if (instance == null)
        {
            // 自身をインスタンスとする
            instance = this;
        }
        else
        {
            // インスタンスが既に存在していたら自身を消去する
            Destroy(gameObject);
        }
    }

    public void OnGoal()
    {
        goalVirtualCamera.enabled = true;

        //ゴールのアニメーションを移行
        GetComponent<Animator>().SetBool("bGoal", true);
    }

    public void GoToSelect()
    {
        TransitionController.instance.SetEventFunction_SceneChangeSetName("Result");
        TransitionController.instance.SetFadeOut();
    }
}
