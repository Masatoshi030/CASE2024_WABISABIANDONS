using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenWallController : MonoBehaviour
{

    Rigidbody[] rigidbodyPartsList = new Rigidbody[4];

    [SerializeField, Header("âÛÇÍÇΩ")]
    bool bBroken = false;

    [SerializeField, Header("è¡Ç¶ÇÈÇ‹Ç≈ÇÃéûä‘")]
    float destroyTimer = 3.0f;

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < transform.childCount; i++) 
        {
            rigidbodyPartsList[i] = transform.GetChild(i).GetComponent<Rigidbody>();
        }
    }

    private void Update()
    {
        if (bBroken)
        {
            destroyTimer -= Time.deltaTime;
            if(destroyTimer < 0.0f)
            {
                Destroy(this.gameObject);
            }
        }
    }

    public void SetBreak()
    {
        bBroken = true;

        for (int i = 0; i < transform.childCount; i++)
        {
            rigidbodyPartsList[i].isKinematic = false;
            rigidbodyPartsList[i].useGravity = true;
            rigidbodyPartsList[i].velocity = new Vector3(Random.Range(-10.0f, 10.0f), Random.Range(-10.0f, 10.0f), Random.Range(-10.0f, 10.0f));
        }
    }
}
