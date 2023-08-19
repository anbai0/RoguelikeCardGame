using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FieldSceneManager : MonoBehaviour
{
    [SerializeField]
    private SceneFader sceneFader;

    string sceneNameToCheck = "ShopScene";

    public void LoadBattleScene(string enemyType)
    {
        if(enemyType == "StrongEnemy")
        {
            StartCoroutine(WaitUnLoadFieldScene());
        }
        else
        {
            // 戦闘シーンをロード
            sceneFader.SceneChange("BattleScene");
        }
    }

    IEnumerator WaitUnLoadFieldScene()
    {
        // バトルシーンをロード
        sceneFader.SceneChange("BattleScene");

        yield return new WaitForSeconds(1.0f);

        //フィールドシーンをアンロード
        sceneFader.SceneChange(unLoadSceneName: "FieldScene");

        if (SceneUtility.GetBuildIndexByScenePath("ShopScene") != -1) //ショップシーンがロードされていた場合
        {
            //ショップシーンをアンロード
            sceneFader.SceneChange(unLoadSceneName: "ShopScene");
        }
    }

    public void LoadBonfireScene()
    {
        // 焚火シーンをロード
        sceneFader.SceneChange("BonfireScene");
    }

    public void LoadTreasureBoxScene()
    {
        // 宝箱シーンをロード
        sceneFader.SceneChange("TreasureBoxScene");
    }

    public void LoadShopScene()
    {
        // ショップシーンをロード
        sceneFader.SceneChange("ShopScene");
    }

    public void ActivateShopScene()
    {
        // ショップシーンのオブジェクトを表示
        sceneFader.ToggleSceneWithFade("ShopScene", true);
    }
}
