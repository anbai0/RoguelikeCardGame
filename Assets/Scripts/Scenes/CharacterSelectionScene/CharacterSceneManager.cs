using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSceneManager : MonoBehaviour
{
    [SerializeField]
    private SceneController sceneController;

    void Update()
    {
        //タイトル画面へ遷移
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //Scene切り替え
            sceneController.sceneChange("TitleScene", "CharacterSelectionScene");
        }
    }

    public void FieldScene()
    {
        sceneController.sceneChange("FieldScene", "CharacterSelectionScene");
    }
}
