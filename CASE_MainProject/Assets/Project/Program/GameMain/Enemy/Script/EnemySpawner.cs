using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : Subscriber
{
    [SerializeField, Header("スポーンさせるオブジェクト")]
    GameObject spawnPrefab;
    [SerializeField, Header("スポーン中のオブジェクト"), ReadOnly]
    GameObject spawnObject;
    Enemy spawnEnemyComponent;

    [SerializeField, Header("スポーン地点までにかける時間")]
    float toSpawnPointSpeed;
    [SerializeField, Header("カウント"), ReadOnly]
    float spawnCnt;

    [SerializeField, Header("スポーン中か"), ReadOnly]
    bool isSpawn = false;

    [SerializeField, Header("スポーン開始地点")]
    Transform spawnStartPoint;
    [SerializeField, Header("スポーン終了")]
    Transform spawnEndPoint;


    private void Update()
    {
        if(isSpawn)
        {
            spawnCnt += Time.deltaTime * (1.0f / toSpawnPointSpeed);
            if (spawnCnt >= 1.0f)
            {
                isSpawn = false;
                spawnCnt = 1.0f;
                spawnEnemyComponent.EnemyRigidbody.isKinematic = false;
                spawnEnemyComponent.EnemyCollider.enabled = true;
                spawnEnemyComponent.EnemyRigidbody.useGravity = true;
            }
            spawnObject.transform.position = Vector3.Lerp(spawnStartPoint.transform.position, spawnEndPoint.transform.position, spawnCnt);
            spawnEnemyComponent.gameObject.transform.position = spawnObject.transform.position;
        }
    }
    public override void ReceiveMsg<T>(Connection sender, int msgType, T msg)
    {
        switch (msgType)
        {
            case 1:
                // スポーン依頼
                isSpawn = true;
                spawnObject = Instantiate(spawnPrefab, spawnStartPoint.position, spawnStartPoint.rotation);
                spawnEnemyComponent = spawnObject.transform.GetChild(0).GetComponent<Enemy>();
                spawnEnemyComponent.Subscribe(observers[0]);
                spawnEnemyComponent.EnemyRigidbody.isKinematic = true;
                spawnEnemyComponent.EnemyRigidbody.useGravity = false;
                spawnEnemyComponent.EnemyCollider.enabled = false;
                spawnCnt = 0.0f;
                break;
        }
    }
}
