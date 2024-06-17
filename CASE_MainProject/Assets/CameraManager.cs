using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class CameraManager : MonoBehaviour
{

    public static CameraManager instance;

    [SerializeField, Header("HitCamera")]
    GameObject hitCamera;

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

    public async void SetHitCamera(float _time)
    {
        //カメラの有効
        hitCamera.SetActive(true);

        // ヒットストップの長さだけ待機
        await Task.Delay((int)(_time * 1000));

        //カメラの無効
        hitCamera.SetActive(false);
    }
}
