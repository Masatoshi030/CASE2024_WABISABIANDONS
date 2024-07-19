using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageJudgeCollision : MonoBehaviour
{

    [SerializeField, Header("このチュートリアルを表示する判定オブジェクト")]
    MessageElementController messageElementController;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            messageElementController.SetActiveTutorial();

            Destroy(transform.parent.gameObject);

            Debug.Log("hitttaa");
        }
    }
}
