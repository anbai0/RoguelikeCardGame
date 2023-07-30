using UnityEngine;
using UnityEngine.SceneManagement;

public class ManagerSceneLoader : MonoBehaviour
{
    private static bool Loaded { get; set; }

    UIManager uiManager;

    void Awake()
    {
        if (Loaded) return;

        Loaded = true;
        SceneManager.LoadScene("ManagerScene", LoadSceneMode.Additive);

        switch (gameObject.name)
        {
            case "TitleSceneManager":
                GetUIManager("Title");
                break;
            case "CharacterSceneManager":
                GetUIManager("Chara");
                break;
            default:
                GetUIManager("OverlayOnly");
                break;
        }

    }

    private void GetUIManager(string uiType)
    {
        //ロード済みのシーンであれば、名前で別シーンを取得できる
        Scene scene = SceneManager.GetSceneByName("ManagerScene");

        //GetRootGameObjectsで、そのシーンのルートGameObjects
        //つまり、ヒエラルキーの最上位のオブジェクトが取得できる
        foreach (var rootGameObject in scene.GetRootGameObjects())
        {
            //Debug.Log(rootGameObject.name);
            UIManager uiManager = rootGameObject.GetComponent<UIManager>();
            if (uiManager != null)
            {
                uiManager.ChangeUI(uiType);
                break;
            }
        }
    }
}