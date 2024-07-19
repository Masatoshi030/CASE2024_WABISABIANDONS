using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageJudgeCollision : MonoBehaviour
{
    bool bHited = false;

    [SerializeField, Header("このチュートリアルを表示する判定オブジェクト")]
    MessageElementController messageElementController;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            if(!bHited)
            {
                messageElementController.SetActiveTutorial();
                bHited = true;
            }

            Destroy(transform.parent.gameObject);

            Debug.Log("hitttaa");
        }
    }
}
