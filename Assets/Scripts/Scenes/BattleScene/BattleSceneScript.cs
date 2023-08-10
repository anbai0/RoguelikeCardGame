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
    }

    public void SwitchSceneAfterLose()
    {
        // フィールドシーンをアンロード
        sceneFader.UnLoadScene("FieldScene");

        // ショップシーンをアンロード
        sceneFader.UnLoadScene("ShopScene");

        // バトルシーンをアンロード、リザルトシーンをロード
        sceneFader.SceneChange("ResultScene", "BattleScene");
    }
}
