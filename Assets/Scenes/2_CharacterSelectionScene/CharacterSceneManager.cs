using UnityEngine;

public class CharacterSceneManager : MonoBehaviour
{

    void Update()
    {
        //タイトル画面へ遷移
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // キャラ選択シーンをアンロードし、タイトルシーンをロード
            SceneFader.Instance.SceneChange("TitleScene", "CharacterSelectionScene");
        }
    }

    public void LoadFieldScene()
    {
        // キャラ選択シーンをアンロードし、フィールドシーンをロード
        SceneFader.Instance.SceneChange("FieldScene", "CharacterSelectionScene",true);
    }
}
