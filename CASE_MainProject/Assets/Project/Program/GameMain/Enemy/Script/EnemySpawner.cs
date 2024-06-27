using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : Subscriber
{
    [SerializeField, Header("�X�|�[��������I�u�W�F�N�g")]
    GameObject spawnPrefab;
    [SerializeField, Header("�X�|�[�����̃I�u�W�F�N�g"), ReadOnly]
    GameObject spawnObject;
    Enemy spawnEnemyComponent;

    [SerializeField, Header("�X�|�[���n�_�܂łɂ����鎞��")]
    float toSpawnPointSpeed;
    [SerializeField, Header("�J�E���g"), ReadOnly]
    float spawnCnt;

    [SerializeField, Header("�X�|�[������"), ReadOnly]
    bool isSpawn = false;

    [SerializeField, Header("�X�|�[���J�n�n�_")]
    Transform spawnStartPoint;
    [SerializeField, Header("�X�|�[���I��")]
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
                // �X�|�[���˗�
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
