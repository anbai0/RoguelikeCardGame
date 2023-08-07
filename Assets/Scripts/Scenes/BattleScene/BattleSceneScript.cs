using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // UnityEngine.SceneManagemntの機能を使用


public class BattleSceneScript : MonoBehaviour
{
    [SerializeField]
    private SceneController sceneController;


    public void SwitchSceneAfterVictory()
    {
        // バトルシーンをアンロード
        sceneController.SceneChange(unLoadSceneName: "BattleScene");
    }

    public void SwitchSceneAfterLose()
    {
        // バトルシーンをアンロード
        sceneController.SceneChange(unLoadSceneName: "BattleScene");

        // フィールドシーンをアンロード
        sceneController.SceneChange(unLoadSceneName: "FieldScene");

        // ショップシーンをアンロード
        sceneController.SceneChange(unLoadSceneName: "ShopScene");

        // リザルトシーンをAdditive
        sceneController.SceneChange("ResultScene");
    }
}
