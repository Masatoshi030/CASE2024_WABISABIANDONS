using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageElementController : MonoBehaviour
{

    [SerializeField, Header("メッセージページリスト")]
    GameObject[] messagePageList;

    [SerializeField, Header("ページカウント")]
    int pageCount = 0;

    [SerializeField, Header("表示中")]
    bool bShow = false;

    GameObject sprits;

    // Start is called before the first frame update
    void Start()
    {
        messagePageList = new GameObject[transform.childCount - 1];

        sprits = transform.GetChild(0).gameObject;
        sprits.SetActive(false);

        for (int i = 1; i < transform.childCount; i++)
        {
            messagePageList[i - 1] = transform.GetChild(i).gameObject;
            messagePageList[i - 1].SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(bShow == false)
        {
            return;
        }

        if(DualSense_Manager.instance.GetInputState().CrossButton == DualSenseUnity.ButtonState.NewDown)
        {
            if (pageCount < messagePageList.Length - 1)
            {
                pageCount++;

                messagePageList[pageCount].SetActive(true);
                messagePageList[pageCount - 1].SetActive(false);
            }
            else
            {
                Destroy(this.gameObject);
            }
        }

        if (DualSense_Manager.instance.GetInputState().CircleButton == DualSenseUnity.ButtonState.NewDown)
        {
            if (pageCount > 0)
            {
                pageCount--;

                messagePageList[pageCount].SetActive(true);
                messagePageList[pageCount + 1].SetActive(false);
            }
        }
    }

    public void SetActiveTutorial()
    {
        bShow = true;
        sprits.SetActive(true);
        messagePageList[pageCount].SetActive(true);
    }
}
