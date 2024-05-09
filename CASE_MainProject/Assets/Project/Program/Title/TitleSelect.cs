using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleSelect : MonoBehaviour
{
    [SerializeField, Header("アニメーター")]
    Animator animController;

    [SerializeField, Header("変更するシーン")]
    public char sceneName;

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
                break;
        }
    }

    void titleStart()
    {
        SceneManager.LoadScene(sceneName);
    }



}
