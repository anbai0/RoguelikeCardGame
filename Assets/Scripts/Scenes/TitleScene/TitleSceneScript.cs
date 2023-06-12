using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class TitleSceneScript : MonoBehaviour
{
    [SerializeField]
    private SceneController sceneController;



    //ボタンをクリックしたら
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //Scene切り替え
            sceneController.sceneChange("CharacterSelectionScene");
        }
    }
}
