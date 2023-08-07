using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FieldSceneScript : MonoBehaviour
{
    [SerializeField]
    private SceneController sceneController;

    public void LoadBattleScene()
    {
        // 戦闘シーンをAdditive
        sceneController.SceneChange("BattleScene");
    }

    public void LoadBonfireScene()
    {
        // 焚火シーンをAdditive
        sceneController.SceneChange("BonfireScene");
    }

    public void LoadShopScene()
    {
        // ショップシーンをAdditive
        sceneController.SceneChange("ShopScene");
    }
}
