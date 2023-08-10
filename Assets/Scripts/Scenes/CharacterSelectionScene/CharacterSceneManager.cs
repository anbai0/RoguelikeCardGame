using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSceneManager : MonoBehaviour
{
    [SerializeField]
    private SceneFader sceneFader;

    void Update()
    {
        //タイトル画面へ遷移
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // キャラ選択シーンをアンロードし、タイトルシーンをロード
            sceneFader.SceneChange("TitleScene", "CharacterSelectionScene");
        }
    }

    public void LoadFieldScene()
    {
        // キャラ選択シーンをアンロードし、フィールドシーンをロード
        sceneFader.SceneChange("FieldScene", "CharacterSelectionScene");
    }
}
