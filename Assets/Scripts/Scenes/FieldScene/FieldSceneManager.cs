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
