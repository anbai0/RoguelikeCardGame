using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FieldSceneManager : MonoBehaviour
{
    [SerializeField]
    private SceneFader sceneFader;

    public void LoadBattleScene()
    {
        // �퓬�V�[�������[�h
        sceneFader.SceneChange("BattleScene");
    }

    public void LoadBonfireScene()
    {
        // ���΃V�[�������[�h
        sceneFader.SceneChange("BonfireScene");
    }

    public void LoadTreasureBoxScene()
    {
        // �󔠃V�[�������[�h
        sceneFader.SceneChange("TreasureBoxScene");
    }

    public void LoadShopScene()
    {
        // �V���b�v�V�[�������[�h
        sceneFader.SceneChange("ShopScene");
    }

    public void ActivateShopScene()
    {
        // �V���b�v�V�[���̃I�u�W�F�N�g��\��
        sceneFader.ToggleSceneWithFade("ShopScene", true);
    }
}
