using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GasSpotController : MonoBehaviour
{

    [SerializeField, Header("範囲設定オブジェクト")]
    GameObject spawnAreaSettingObject;

    Vector3 spawnAreaMin, spawnAreaMax;

    [SerializeField, Header("生成する爆発ポイント")]
    GameObject explosionPoint;

    [SerializeField, Header("生成クールダウンタイム")]
    float spawnTime = 0.5f;

    [SerializeField, Header("生成クールダウンタイマー"), ReadOnly]
    float spawnTimer = 0.0f;

    [SerializeField, Header("一回での生成個数")]
    int spawnCount = 2;

    [SerializeField, Header("生成有効")]
    bool bSpawnEnable = true;

    private void Awake()
    {
        //実行すると消える
        if(this.enabled)
            spawnAreaSettingObject.GetComponent<MeshRenderer>().enabled = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        //生成範囲の計算
        spawnAreaMin = spawnAreaSettingObject.transform.position - spawnAreaSettingObject.transform.localScale * 0.5f;
        spawnAreaMax = spawnAreaSettingObject.transform.position + spawnAreaSettingObject.transform.localScale * 0.5f;
    }

    // Update is called once per frame
    void Update()
    {
        if (bSpawnEnable)
        {
            spawnTimer += Time.deltaTime;
            if(spawnTimer > spawnTime)
            {
                //設定個数
                for (int i = 0; i < spawnCount; i++)
                {
                    //爆発ポイント生成座標をランダムで指定
                    Vector3 spawnPosition = new Vector3(
                        Random.Range(spawnAreaMin.x, spawnAreaMax.x),
                        Random.Range(spawnAreaMin.y, spawnAreaMax.y),
                        Random.Range(spawnAreaMin.z, spawnAreaMax.z)
                        );

                    //爆発ポイント生成
                    Instantiate(explosionPoint, spawnPosition, Quaternion.identity);
                }

                //タイマーのリセット
                spawnTimer = 0.0f;
            }
        }
    }
}
