using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // UnityEngine.SceneManagemnt�̋@�\���g�p


public class ResultSceneScript : MonoBehaviour
{
    [SerializeField]
    private SceneController sceneController;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //Scene�؂�ւ�
            sceneController.SceneChange("TitleScene");
        }
    }

    public void LoadTitleScene()
    {
        // Manager�V�[�����A�����[�h
        sceneController.SceneChange(unLoadSceneName: "ManagerScene");

        // ���U���g�V�[�����A�����[�h
        sceneController.SceneChange(unLoadSceneName: "ResultScene");

        // Title�V�[����Additive
        sceneController.SceneChange("TitleScene");
    }

}
