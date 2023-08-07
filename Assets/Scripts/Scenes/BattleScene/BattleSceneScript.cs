using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // UnityEngine.SceneManagemnt�̋@�\���g�p


public class BattleSceneScript : MonoBehaviour
{
    [SerializeField]
    private SceneController sceneController;


    public void SwitchSceneAfterVictory()
    {
        // �o�g���V�[�����A�����[�h
        sceneController.SceneChange(unLoadSceneName: "BattleScene");
    }

    public void SwitchSceneAfterLose()
    {
        // �o�g���V�[�����A�����[�h
        sceneController.SceneChange(unLoadSceneName: "BattleScene");

        // �t�B�[���h�V�[�����A�����[�h
        sceneController.SceneChange(unLoadSceneName: "FieldScene");

        // �V���b�v�V�[�����A�����[�h
        sceneController.SceneChange(unLoadSceneName: "ShopScene");

        // ���U���g�V�[����Additive
        sceneController.SceneChange("ResultScene");
    }
}
