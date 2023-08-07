using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TitleSceneManager : MonoBehaviour
{
    [SerializeField]
    private SceneController sceneController;


    public void LoadCharaSelectScene()
    {
        // タイトルシーンをアンロードし、キャラ選択シーンをAdditive
        sceneController.SceneChange("CharacterSelectionScene", "TitleScene");
    }
}
