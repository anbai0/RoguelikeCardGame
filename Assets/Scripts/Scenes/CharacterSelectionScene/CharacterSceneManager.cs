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
            // キャラ選択シーンをアンロードし、タイトルシーンをAdditive
            sceneController.SceneChange("TitleScene", "CharacterSelectionScene");
        }
    }

    public void LoadFieldScene()
    {
        // キャラ選択シーンをアンロードし、フィールドシーンをAdditive
        sceneController.SceneChange("FieldScene", "CharacterSelectionScene");
    }
}
