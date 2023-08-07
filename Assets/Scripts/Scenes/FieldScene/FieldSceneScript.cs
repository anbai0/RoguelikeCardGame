using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FieldSceneScript : MonoBehaviour
{
    [SerializeField]
    private SceneController sceneController;

    public void LoadBattleScene()
    {
        // �퓬�V�[����Additive
        sceneController.SceneChange("BattleScene");
    }

    public void LoadBonfireScene()
    {
        // ���΃V�[����Additive
        sceneController.SceneChange("BonfireScene");
    }

    public void LoadShopScene()
    {
        // �V���b�v�V�[����Additive
        sceneController.SceneChange("ShopScene");
    }
}
