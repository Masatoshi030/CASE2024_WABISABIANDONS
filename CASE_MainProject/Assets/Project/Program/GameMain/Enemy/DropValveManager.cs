using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropValveManager : MonoBehaviour
{
    public static DropValveManager instance;

    [SerializeField, Header("�o���u�G�~�b�^�[")]
    GameObject valveEmitter;

    [SerializeField, Header("���x")]
    float speed = 1.0f;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void CreateValves(uint spawnNum, Vector3 spawnPosition, bool isAuto)
    {
        GameObject obj = Instantiate(valveEmitter, spawnPosition, Quaternion.identity);
        obj.transform.SetParent(transform);
        obj.GetComponent<ValveEmitter>().Create(spawnNum, speed, isAuto);
    }
}
