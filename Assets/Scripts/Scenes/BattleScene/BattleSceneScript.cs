using SelfMadeNamespace;
using UnityEngine;
using UnityEngine.SceneManagement;


public class BattleSceneScript : MonoBehaviour
{
    [SerializeField]
    private SceneFader sceneFader;

    public void SwitchSceneAfterVictory()
    {
        // バトルシーンをアンロード
        sceneFader.SceneChange(unLoadSceneName: "BattleScene");

        PlayerController.isPlayerActive = true;     // プレイヤーを動けるようにする
    }

    public void SwitchSceneAfterLose()
    {
        // バトルシーンをアンロード、リザルトシーンをロード
        sceneFader.SceneChange("ResultScene", "BattleScene");
    }
}