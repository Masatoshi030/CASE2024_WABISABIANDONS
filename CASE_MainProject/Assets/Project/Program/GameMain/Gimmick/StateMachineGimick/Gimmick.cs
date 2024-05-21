using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gimmick : MonoBehaviour
{
    [SerializeField, Header("ó‘Ô"), ReadOnly]
    string stateName;
    public string StateName { get => stateName; set => stateName = value; }

    GimmickStateMachine gimmickStateMachine;
    public GimmickStateMachine Machine { get => gimmickStateMachine; }

    Animator animator;
    public Animator GimmickAnimator { get => animator; }

    bool isOperational = true;

    void Start()
    {
        animator = GetComponent<Animator>();
        gimmickStateMachine = GetComponent<GimmickStateMachine>();
        gimmickStateMachine.GimmickTarget = this;
        gimmickStateMachine.Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        gimmickStateMachine.MainFunc();
    }

    private void OnCollisionEnter(Collision collision)
    {
        gimmickStateMachine.CollisionEnterSelf(collision.gameObject);
    }

    private void OnCollisionStay(Collision collision)
    {
        gimmickStateMachine.CollisionStaySelf(collision.gameObject);
    }

    private void OnCollisionExit(Collision collision)
    {
        gimmickStateMachine.CollisionExitSelf(collision.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        gimmickStateMachine.TriggerEnterSelf(other.gameObject);
    }

    private void OnTriggerStay(Collider other)
    {
        gimmickStateMachine.TriggerStaySelf(other.gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        gimmickStateMachine.TriggerExitSelf(other.gameObject);
    }
}
