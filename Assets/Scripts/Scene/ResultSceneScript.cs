using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // UnityEngine.SceneManagemntの機能を使用


public class ResultSceneScript : MonoBehaviour
{
    [SerializeField]
    private SceneController sceneController;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //Scene切り替え
            sceneController.sceneChange("FieldScene");
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            //Scene切り替え
            sceneController.sceneChange("TitleScene");
        }
    }
}
