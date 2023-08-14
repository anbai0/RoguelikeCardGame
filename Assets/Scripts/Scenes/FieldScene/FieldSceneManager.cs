using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FieldSceneManager : MonoBehaviour
{
    [SerializeField]
    private SceneFader sceneFader;

    public void LoadBattleScene()
    {
        // 戦闘シーンをロード
        sceneFader.SceneChange("BattleScene");
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
