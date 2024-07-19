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

    [SerializeField, Header("SEクリップ")]
    AudioClip[] clips;

    AudioSource audioSource;

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

        audioSource = GetComponent<AudioSource>();
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
                audioSource.PlayOneShot(clips[1]);
            }
            else
            {
                MessageManager.instance.ClearElement();
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
                audioSource.PlayOneShot(clips[1]);
            }
        }
    }

    public void SetActiveTutorial()
    {
        MessageManager.instance.SetCurrentMessage(this);
        bShow = true;
        sprits.SetActive(true);
        audioSource.PlayOneShot(clips[0]);
        messagePageList[pageCount].SetActive(true);
    }

    public void SetInactiveTutorial()
    {
        bShow = false;
        sprits.SetActive(false);
        messagePageList[pageCount].SetActive(false);
        Destroy(this.gameObject);
    }
}
