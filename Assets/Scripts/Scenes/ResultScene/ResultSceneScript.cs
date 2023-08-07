using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // UnityEngine.SceneManagemntの機能を使用


public class ResultSceneScript : MonoBehaviour
{
    [SerializeField]
    private SceneController sceneController;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //Scene切り替え
            sceneController.SceneChange("TitleScene");
        }
    }

    public void LoadTitleScene()
    {
        // Managerシーンをアンロード
        sceneController.SceneChange(unLoadSceneName: "ManagerScene");

        // リザルトシーンをアンロード
        sceneController.SceneChange(unLoadSceneName: "ResultScene");

        // TitleシーンをAdditive
        sceneController.SceneChange("TitleScene");
    }

}
