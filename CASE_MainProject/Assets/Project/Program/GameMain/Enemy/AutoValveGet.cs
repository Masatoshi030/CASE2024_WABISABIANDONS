using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoValveGet : MonoBehaviour
{
    public bool isAuto = false;
    public float waitTime = 1.0f;
    float cnt = 0.0f;

    [SerializeField, Header("RigidBody")]
    Rigidbody rb;
    [SerializeField, Header("コライダー")]
    Collider myCollider;

    private void Start()
    {
        
    }
    void Update()
    {
        cnt += Time.deltaTime;
        if(cnt >= waitTime && isAuto)
        {
            gameObject.GetComponent<GoldValveController>().GetGoldValve();
        }
        if(cnt >= 0.5f)
        {
            myCollider.enabled = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Ground")
        {
            rb.isKinematic = true;
            GetComponent<GoldValveController>().startPosition = transform.position;
        }
    }
}
