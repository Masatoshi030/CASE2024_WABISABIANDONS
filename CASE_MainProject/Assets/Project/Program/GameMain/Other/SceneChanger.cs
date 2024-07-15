using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Threading.Tasks;

public class SceneChanger : MonoBehaviour
{

    public string changeSceneName = "Title";

    public GameObject loadingUI;

    [SerializeField, Header("ローディングの最低時間")]
    float loadingShortestTime = 1.0f;

    public bool bLoadingShortest = false;

    private void Start()
    {
        if (loadingUI != null)
        {
            loadingUI.gameObject.SetActive(false);
        }
    }

    public void SceneChange(string sceneName)
    {
        StartCoroutine(SceneChange_Async(sceneName));
    }

    IEnumerator SceneChange_Async(string sceneName)
    {
        //=== ロード開始 ===//

        //シーンをロード開始する
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        //シーンの切り替えを遅延させる
        asyncLoad.allowSceneActivation = false;

        if (loadingUI != null)
        {
            loadingUI.gameObject.SetActive(true);
        }

        LoadingUIController loadingUIController_buf = loadingUI.GetComponent<LoadingUIController>();

        //最短ロードタイマー計測開始
        LoadingShortestTimeCounter();


        //=== ロード中 ===//

        while (!asyncLoad.isDone && bLoadingShortest == false)
        {

            if (loadingUI != null)
            {
                loadingUIController_buf.SetActiveMark((int)(asyncLoad.progress / 0.3f));
            }

                yield return null;
        }


        //=== ロード完了 ===//

        Debug.Log("ロード完了");

        if (loadingUI != null)
        {
            loadingUI.gameObject.SetActive(false);
        }

        //シーンを切り替える
        asyncLoad.allowSceneActivation = true;
    }

    async void LoadingShortestTimeCounter()
    {
        // 指定時間待機
        await Task.Delay((int)(loadingShortestTime * 1000));

        bLoadingShortest = true;
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
