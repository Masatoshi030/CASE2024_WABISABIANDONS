using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleSelect : MonoBehaviour
{
    [SerializeField, Header("アニメーター")]
    Animator animController;

    [SerializeField, Header("変更するシーン")]
    public string sceneName;

    private enum title_State
    {
        Start,
        Menu,
    }

    private title_State scene_now;

    // Start is called before the first frame update
    void Start()
    {
        scene_now =title_State.Start;  //最初の設定
        animController = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        switch(scene_now)
        {
            case title_State.Start:
                titleStart();
                break;
            case title_State.Menu:
                titleMenu();
                break;
        }
    }

    void titleStart()
    {
        if(Input.GetKeyDown(KeyCode.LeftArrow))
        {
            scene_now = title_State.Menu;
            animController.SetTrigger("Menu_Trigger");
        }

        if(Input.GetKeyDown(KeyCode.Return))
        {
            SceneManager.LoadScene(sceneName);
        }
    }

    void titleMenu()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            scene_now = title_State.Start;
            animController.SetTrigger("Start_Trigger");
        }

    }



}
