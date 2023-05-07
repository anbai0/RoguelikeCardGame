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
            sceneController.sceneChange("FieldScene");
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            //Scene�؂�ւ�
            sceneController.sceneChange("TitleScene");
        }
    }
}
