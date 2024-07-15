using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneChanger : MonoBehaviour
{

    public string changeSceneName = "Title";

    public GameObject loadingUI;

    private void Start()
    {
        loadingUI.gameObject.SetActive(false);
    }

    public void SceneChange(string sceneName)
    {
        StartCoroutine(SceneChange_Async(sceneName));
    }

    IEnumerator SceneChange_Async(string sceneName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        loadingUI.gameObject.SetActive(true);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        loadingUI.gameObject.SetActive(false);
    }

    public void Reload()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void SceneChange_ConfiguredScene()
    {
        SceneChange(changeSceneName);
    }
}
