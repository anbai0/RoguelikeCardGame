using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class ManagerSceneLoader : MonoBehaviour
{
    private static bool Loaded { get; set; }

    GameManager gameManager;

    void Awake()
    {
        if (Loaded)
        {
            // 今いるシーンに応じてUIを切り替える
            GetCurrentSceneName();
            return;
        }       

        Loaded = true;
        StartCoroutine(LoadManagerScene()); // コルーチンの呼び出し
    }

    private IEnumerator LoadManagerScene()
    {
        // ManagerSceneを非同期でロード
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("ManagerScene", LoadSceneMode.Additive);

        // ロードが完了するまで待機
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        // ManagerSceneがロードされた後にGetCurrentSceneNameでGetUIManagerを呼び出す
        GetCurrentSceneName();
    }

    /// <summary>
    /// 今いるシーンを特定し、シーンに応じて処理します。
    /// </summary>
    void GetCurrentSceneName()
    {
        switch (gameObject.name)
        {
            case "TitleSceneManager":
                GetUIManager("Title");
                break;
            case "CharacterSceneManager":
                GetUIManager("None");
                break;
            case "ResultSceneManager":
                GetUIManager("None");
                break;
            default:
                GetUIManager("OverlayOnly");
                break;
        }
    }

    /// <summary>
    /// ManagerSceneからUIManagerを取得し、UIManagerのChangeUIメソッドを使い、UIを切り替えます。
    /// </summary>
    /// <param name="uiType">ChangeUIに使う引数</param>
    private void GetUIManager(string uiType)
    {
        Scene scene = SceneManager.GetSceneByName("ManagerScene");

        foreach (var rootGameObject in scene.GetRootGameObjects())
        {
            UIManager uiManager = rootGameObject.GetComponent<UIManager>();
            if (uiManager != null)
            {
                uiManager.ChangeUI(uiType);
                break;
            }
        }
    }

    public GameManager GetGameManager()
    {
        Scene scene = SceneManager.GetSceneByName("ManagerScene");

        foreach (var rootGameObject in scene.GetRootGameObjects())
        {
            gameManager = rootGameObject.GetComponent<GameManager>();
            return gameManager;
        }
        Debug.LogError("GameManagerを取得できませんでした。");
        return null;
    }

}
